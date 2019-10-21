# Protecting Secrets when Building Docker Images

Protecting secrets (passwords, tokens, etc.) is vital when building Docker images. You do not want to be in a situation where a consumer of your image has access to data they should not have. But there are many cases where your Dockerfile requires access to data like credentials. Providing your Dockerfile with access to the secret data it requires can cause it to be exposed in the resulting image if you're not careful. You must be familiar with how Docker images are built so that you can protect that data appropriately.

There are two aspects to be aware of when it comes to protecting secrets:
1. Exposing secrets in an image that is published to a registry. This is the most obvious scenario. You don't want consumers of your image to have access to data they should not have.
2. Exposing secrets locally on the Docker host machine. This isn't necessarily an issue; it depends on your build processes. Just be aware that while your published image may not be exposing sensitive data, there can be sensitive data stored on the Docker host machine that you may want to clean up depending on how the machine is managed in your build workflow. For example, when using Azure DevOps pipelines with a hosted agent, there's no need to clean up data because the virtual machine of the build agent is automatically deleted at the end of the build. But if you manage your own agent, you're in charge of cleaning any leftover state should it be necessary.

## Docker Layers

The concept of Docker layers is much too complex of a topic to discuss here so please read the [Docker documentation on layers](https://docs.docker.com/storage/storagedriver/) if you're not already familiar with them. The key thing to understand about a Docker image is that all of the layers from which it is composed are available to be peeked at. If a file was added in one layer and then deleted in a subsequent layer, that file may not exist in the resulting container layer but it still exists in the layer that added the file. And each of an image's layers are accessible to consumers of the image.

### Example: Exposing Secret Data

This example shows a file with secrets being copied into the image and then being deleted in a subsequent layer. The resulting container layer does not contain the file but, as is shown, the file is still accessible from the image's layers.

```
# Dockerfile
FROM mcr.microsoft.com/dotnet/core/sdk:3.0

COPY ./secret.txt .
COPY ./project ./project

RUN dotnet build project/MyProject.csproj -v quiet -p:SecretFile=secret.txt && \
    rm secret.txt
```

```
$ docker build -t example .
Sending build context to Docker daemon  5.632kB
Step 1/4 : FROM mcr.microsoft.com/dotnet/core/sdk:3.0
 ---> 4422e7fb740c
Step 2/4 : COPY ./secret.txt .
 ---> 36ae62f1ce14
Step 3/4 : COPY ./project ./project
 ---> 93b67f722b75
Step 4/4 : RUN dotnet build project/MyProject.csproj -v quiet -p:SecretFile=secret.txt &&     rm secret.txt
 ---> Running in c8b157f236fb
Microsoft (R) Build Engine version 16.3.0+0f4c62fea for .NET Core
Copyright (C) Microsoft Corporation. All rights reserved.

Build succeeded.
    0 Warning(s)
    0 Error(s)

Time Elapsed 00:00:01.79
Removing intermediate container c8b157f236fb
 ---> 3a8436f9b702
Successfully built 3a8436f9b702
Successfully tagged example:latest

$ docker run --rm example cat secret.txt
cat: secret.txt: No such file or directory

$ docker run --rm 36ae62f1ce14 cat secret.txt
my_secret
```

A better way to handle this would be to use a [multi-stage build](#multi-stage-builds) or [BuildKit secrets](#buildkit-secrets).

## ARG Instruction

The [`ARG` instruction](https://docs.docker.com/engine/reference/builder/#arg) provides a way to pass in values that the Dockerfile can consume at build time. The important thing to realize about the `ARG` instruction is that `ARG` values are embedded into the resulting image. An easy way to visualize this is to view the image layers with the `docker history` command (see the example below). That's not to say that using the `ARG` instruction is discouraged for secrets. They're actually quite useful and a great way to consume secrets without exposing them within the Dockerfile itself. But they should be used in conjunction with [multi-stage builds](#multi-stage-builds) in order to avoid leaking secrets within your published images.

### Example

In this example, an `ARG` is passed a secret value and then used by a `RUN` command. By referencing the environment variable in a `RUN` command, the value shows up in `docker history`.

```
# Dockerfile
FROM mcr.microsoft.com/dotnet/core/sdk:3.0
ARG SECRET
COPY ./project ./project
RUN dotnet build project/MyProject.csproj -v quiet -p:Secret=$SECRET
```

```
$ docker build --build-arg SECRET=my_secret -t example .
Sending build context to Docker daemon  4.608kB
Step 1/4 : FROM mcr.microsoft.com/dotnet/core/sdk:3.0
 ---> 4422e7fb740c
Step 2/4 : ARG SECRET
 ---> Running in 3f38aad10f98
Removing intermediate container 3f38aad10f98
 ---> f1710c190d3c
Step 3/4 : COPY ./project ./project
 ---> 9d4107a0b039
Step 4/4 : RUN dotnet build project/MyProject.csproj -v quiet -p:Secret=$MY_SECRET
 ---> Running in 16fdf6b65126
Microsoft (R) Build Engine version 16.3.0+0f4c62fea for .NET Core
Copyright (C) Microsoft Corporation. All rights reserved.


Build succeeded.
    0 Warning(s)
    0 Error(s)

Time Elapsed 00:00:01.73
Removing intermediate container 16fdf6b65126
 ---> 502fe90746d3
Successfully built 502fe90746d3
Successfully tagged example:latest

$ docker history --format "{{ .CreatedBy }}" example --no-trunc
|1 SECRET=my_secret /bin/sh -c dotnet build project/MyProject.csproj -v quiet -p:Secret=$MY_SECRET
/bin/sh -c #(nop) COPY dir:8eb964fe209560f768f5bb33dc5e599255dd02b8cb6fcf2ed5ed1f0eaf1c3545 in ./project
/bin/sh -c #(nop)  ARG SECRET
...
```

## Multi-Stage Builds

The next concept builds upon the idea of Docker layers. [Multi-stage builds](https://docs.docker.com/develop/develop-images/multistage-build/) are a mechanism for authoring Dockerfiles in multiple stages with each stage having its own base image and instructions. Files from an earlier stage can be explicitly copied to a later stage. The canonical scenario of multi-stage builds is a stage that builds the application and a final stage that runs the application. This allows the stage which builds the application to use an "SDK" base image and install any special tools required to build the application. And the final stage can be a slimmed down image that contains only the necessary files for running the application. This helps to keep the built image as small as possible.

Multi-stage builds are a great way to avoid exposing secrets in the final image if you only consume those secrets in intermediate stages. _Only the layers produced by the final stage end up in the resulting image._ That's an important thing to keep in mind when thinking about ways to avoid having secrets leak out. However, secrets can still being exposed locally on the Docker host machine by any of the layers built in any of the build stages. Those layers still exist on disk and can expose secrets locally on that machine as shown in the last command of the example below. You'll need to determine whether this is an issue in the context of your build environment and clean things up as appropriate.

### Example

This example shows how the use of multi-stage builds can prevent data from being leaked in the resulting image. The final stage copies in the built app files from the base stage but does not make use of the `SECRET` argument. Because of that, the resulting image doesn't expose the secret when running `docker history`. While the resulting image that would get published is safe from exposing the secret, the intermediate layer that exists locally on the Docker host _does_ expose the value of the `SECRET` argument.

```
# Dockerfile
FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS base
ARG SECRET
COPY ./project ./project
RUN dotnet build project/MyProject.csproj -v quiet -p:Secret=$SECRET -o app


FROM mcr.microsoft.com/dotnet/core/runtime:3.0

COPY --from=base ./app ./app
```

```
$ docker build -t example --build-arg SECRET=mysecret .
Sending build context to Docker daemon  4.608kB
Step 1/6 : FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS base
 ---> 4422e7fb740c
Step 2/6 : ARG SECRET
 ---> Running in 8777b4e42fd6
Removing intermediate container 8777b4e42fd6
 ---> 512c17ac56e3
Step 3/6 : COPY ./project ./project
 ---> 051dbccd6a4f
Step 4/6 : RUN dotnet build project/MyProject.csproj -v quiet -p:Secret=$SECRET -o app
 ---> Running in 2cf986e25979
Microsoft (R) Build Engine version 16.3.0+0f4c62fea for .NET Core
Copyright (C) Microsoft Corporation. All rights reserved.


Build succeeded.
    0 Warning(s)
    0 Error(s)

Time Elapsed 00:00:01.75
Removing intermediate container 2cf986e25979
 ---> 027eb010bf53
Step 5/6 : FROM mcr.microsoft.com/dotnet/core/runtime:3.0
 ---> f5ad033cd99e
Step 6/6 : COPY --from=base ./app ./app
 ---> c81a374d5d72
Successfully built c81a374d5d72
Successfully tagged example:latest

$ docker history --format "{{ .CreatedBy }}" example --no-trunc
/bin/sh -c #(nop) COPY dir:9bcc897e44e896170d95b122f55bd7e8becc3ef59cf497b9177b595ff713733c in ./app
...

$ docker history --format "{{ .CreatedBy }}" 027eb010bf53 --no-trunc
|1 SECRET=mysecret /bin/sh -c dotnet build project/MyProject.csproj -v quiet -p:Secret=$SECRET -o app
/bin/sh -c #(nop) COPY dir:8eb964fe209560f768f5bb33dc5e599255dd02b8cb6fcf2ed5ed1f0eaf1c3545 in ./project
/bin/sh -c #(nop)  ARG SECRET
...
```

## BuildKit Secrets
For scenarios where you have secrets already stored on disk on the Docker host machine, such as a private RSA key, you can make use of a [feature](https://docs.docker.com/develop/develop-images/build_enhancements/#new-docker-build-secret-information) in Docker's [BuildKit](https://github.com/moby/buildkit) implementation for passing files securely in a `docker build` command. This feature is currently only supported for building Linux containers. It avoids storing secrets in any of the built Docker layers, including when using multi-stage builds. But, of course, the file does exist on the Docker host machine and needs to be properly secured to prevent any unauthorized access.

The feature is used by specifying a special type of mount, named `secret`, on a `RUN` instruction. This is configured to reference a secret by ID that corresponds to the secret that is specified in the `docker build` command. During the context of the `RUN` instruction the secret file is mounted and made available. When the `RUN` instruction finishes, the contents of the secret file are cleared. This is done before the layer is written which means the secret is not exposed in the intermediate layer.

The Dockerfile needs to make use of a special syntax to describe the secret it's attempting to access. The Dockerfile must enable the new syntax features by overriding the default frontend (see the [documentation](https://docs.docker.com/develop/develop-images/build_enhancements/#overriding-default-frontends) for more info). Next, it uses the `--mount-type-secret` [syntax](https://docs.docker.com/develop/develop-images/build_enhancements/#new-docker-build-secret-information) to access the secret.

Example Dockerfile:
```
# syntax = docker/dockerfile:1.0-experimental

FROM debian

RUN --mount=type=secret,id=id_rsa,dst=~/.ssh/id_rsa \
    curl -u <username>: --key ~/ssh/id_rsa <url>
```

When building this Dockerfile, BuildKit must be enabled and the `--secret` option is used to provide the path to the `id_rsa` file:

```
DOCKER_BUILDKIT=1 docker build --secret id=id_rsa,src=~/.ssh/id_rsa -t example --progress=plain .
```

## Cleaning the Build Results
As mentioned previously, the act of building Docker images can leave secret data on the Docker host machine even if the image being published is free of any secrets. Depending on your build environment, this may or may not be an issue. If you've determined that you do need to clean the results of the build to ensure no secrets are left in any Docker artifacts, here are some commands that can be helpful:

* Remove any container using the image: `docker rm -f <CONTAINER NAME>`
  An image cannot be deleted if it's being used by any containers so you'll first need to make sure the containers are deleted. See the [documentation](https://docs.docker.com/engine/reference/commandline/rm/).
* Remove a specific image: `docker rmi -f <IMAGE ID>`
  Note that an image consists of intermediate layers, each having their own image ID. You need to delete all of them in order to completely delete all layers associated with the image. You can find the image IDs of an image's intermediate layers by running `docker history <IMAGE ID> --format "{{ .ID }}"`. See the [documentation](https://docs.docker.com/engine/reference/commandline/rmi/).
* Remove all unused data: `docker system prune -a -f`
  This is a heavy-handed way to delete all unused data in your Docker environment, including containers, networks, and images. See the [documentation](https://docs.docker.com/engine/reference/commandline/system_prune/).
