# Protecting Secrets when Building Docker Images

Protecting secrets (passwords, tokens, etc.) is vital when building Docker images. You do not want to be in a situation where a consumer of your image has access to data they should not have. But there are many cases where your Dockerfile requires access to data like credentials in order to execute. Providing your Dockerfile with access to the secret data it requires can cause it to be exposed if you're not careful. You must be familiar with how Docker images are built so that you can protect that data appropriately.

There are two aspects to be aware of when it comes to protecting secrets:
1. Exposing secrets in an image that is published to a registry. This is the most obvious scenario. You don't want consumers of your image to have access to data they should not have.
2. Exposing secrets locally on the Docker host machine. This isn't necessarily an issue; it depends on your build processes. Just be aware that while your published image may not be exposing sensitive data, there can be sensitive data stored on the Docker host machine that you may want to clean up depending on how the machine is managed in your build workflow. For example, when using Azure DevOps pipelines with a hosted agent, there's no need to clean up data because the virtual machine of the build agent is automatically deleted at the end of the build. But if you manage your own agent, you're in charge of cleaning any leftover state should it be necessary.

## Docker Layers

The concept of Docker layers is much too complex of a topic to discuss here so please read the [Docker documentation on layers](https://docs.docker.com/storage/storagedriver/) if you're not already familiar with them. The key thing to understand about a Docker image is that all of the layers from which it is composed are available to be peeked at. If a file was added in one layer and then deleted in a subsequent layer, that file may not exist in the resulting container layer but it still exists in the layer that added the file. And each of an image's layers are accessible to consumers of the image.

### Example: Exposing Secret Data

This example shows a file being added in one layer and then being removed in a subsequent layer. The resulting container layer does not contain the file but, as is shown, the file is still accessible from the image's layers.

```
# Dockerfile
FROM debian

RUN echo secret > secret.txt
RUN rm secret.txt
```

```
$ docker build -t example .
Sending build context to Docker daemon  2.048kB
Step 1/3 : FROM debian
 ---> c2c03a296d23
Step 2/3 : RUN echo secret > secret.txt
 ---> Running in daf7a288cd7a
Removing intermediate container daf7a288cd7a
 ---> 7476f82445c5
Step 3/3 : RUN rm secret.txt
 ---> Running in c08a7ec9bf36
Removing intermediate container c08a7ec9bf36
 ---> f04de0f9bc5e
Successfully built f04de0f9bc5e
Successfully tagged example:latest

$ docker run --rm example cat secret.txt
cat: secret.txt: No such file or directory

$ docker run --rm 7476f82445c5 cat secret.txt
secret
```

#### Example: Hiding Secret Data

This example shows a file with a secret being added and then removed in one layer. In Docker, the result of the file system changes are not written to a layer until the entire `RUN` operation has been executed. In this example, the file has been added and then removed in one shell command; therefore, the resulting layer doesn't contain the secret file.

```
# Dockerfile
FROM debian

RUN echo secret > secret.txt \
    && rm secret.txt
```

```
$ docker build -t example .
Sending build context to Docker daemon  2.048kB
Step 1/2 : FROM debian
 ---> c2c03a296d23
Step 2/2 : RUN echo secret > secret.txt     && rm secret.txt
 ---> Running in debb237b22bc
Removing intermediate container debb237b22bc
 ---> 0d78fad0dde3
Successfully built 0d78fad0dde3
Successfully tagged example:latest

$ docker run --rm example cat secret.txt
cat: secret.txt: No such file or directory

$ docker run --rm 0d78fad0dde3 cat secret.txt
cat: secret.txt: No such file or directory
```

## ARG Instruction

The [`ARG` instruction](https://docs.docker.com/engine/reference/builder/#arg) provides a way to pass in values that the Dockerfile can consume at build time. The important thing to realize about the `ARG` instruction is that `ARG` values are embedded into the resulting image. An easy way to visualize this is to view the image layers with the `docker history` command (see the example below). That's not to say that using the `ARG` instruction is discouraged for secrets. They're actually quite useful and a great way to consume secrets without exposing them within the Dockerfile itself. But they should be used in conjunction with [multi-stage builds](#multi-stage-builds) in order to avoid leaking secrets within your published images.

### Example

In this example, an `ARG` is passed a secret value and then used by a `RUN` command. The fact that the secret is stored in a file is irrelevant; all that matters is that the `MYSECRET` environment variable is referenced in the `RUN` command. Because it is referenced, the value shows up in `docker history`.

```
# Dockerfile
FROM debian
ARG MY_SECRET
RUN echo $MY_SECRET > secret.txt
```

```
$ docker build . --build-arg MY_SECRET=foo -t example
Sending build context to Docker daemon  2.048kB
Step 1/3 : FROM debian
 ---> c2c03a296d23
Step 2/3 : ARG MY_SECRET
 ---> Running in 45a50c5a1702
Removing intermediate container 45a50c5a1702
 ---> df8f31db64e6
Step 3/3 : RUN echo $MY_SECRET > secret.txt
 ---> Running in f30be4520a41
Removing intermediate container f30be4520a41
 ---> f855f82a2430
Successfully built f855f82a2430
Successfully tagged example:latest

$ docker history --format "{{ .CreatedBy }}" example --no-trunc
|1 MY_SECRET=foo /bin/sh -c echo $MY_SECRET > secret.txt
/bin/sh -c #(nop)  ARG MY_SECRET
/bin/sh -c #(nop)  CMD ["bash"]
/bin/sh -c #(nop) ADD file:770e381defc5e4a0ba5df52265a96494b9f5d94309234cb3f7bc6b00e1d18f9a in /
```

## Multi-Stage Builds

The next concept builds upon the idea of Docker layers. [Multi-stage builds](https://docs.docker.com/develop/develop-images/multistage-build/) are a mechanism for authoring Dockerfiles in multiple stages with each stage having its own base image and instructions. Files from an earlier stage can be explicitly copied to a later stage. The canonical scenario of multi-stage builds is a stage that builds the application and a final stage that runs the application. This allows the stage which builds the application to use an "SDK" base image and install any special tools required to build the application. And the final stage can be a slimmed down image that contains only the necessary files for running the application. This helps to keep the built image as small as possible.

Multi-stage builds are a great way to avoid exposing secrets in the final image if you only consume those secrets in intermediate stages. _Only the layers produced by the final stage end up in the resulting image._ That's an important thing to keep in mind when thinking about ways to avoid having secrets leak out. However, secrets can still being exposed locally on the Docker host machine by any of the layers built in any of the build stages. Those layers still exist on disk and can expose secrets locally on that machine as shown in the last command of the example below. You'll need to determine whether this is an issue in the context of your build environment and clean things up as appropriate.

### Example

This example shows how the use of multi-stage builds can prevent data from being leaked in the resulting image. The final stage copies in the `bar.txt` file from the base stage but does not copy the `foo.txt` file. Because of that, the resulting image doesn't contain the `foo.txt` file. Also, even though the secret value is passed in the `--build-arg` for the `docker build` command, the `docker history` of the resulting image doesn't expose that value; it only shows the layers of the final stage none of which make use the `MY_SECRET` `ARG`.

```
# Dockerfile
FROM debian as base

ARG MY_SECRET

RUN echo $MY_SECRET > foo.txt
RUN echo bar > bar.txt


FROM debian as final

COPY --from=base bar.txt .
```

```
$ docker build . -t example --build-arg MY_SECRET=foo
Sending build context to Docker daemon  2.048kB
Step 1/6 : FROM debian as base
 ---> c2c03a296d23
Step 2/6 : ARG MY_SECRET
 ---> Running in 5613369a10be
Removing intermediate container 5613369a10be
 ---> 634974d9d046
Step 3/6 : RUN echo $MY_SECRET > foo.txt
 ---> Running in 81cee1021639
Removing intermediate container 81cee1021639
 ---> 46837435cff7
Step 4/6 : RUN echo bar > bar.txt
 ---> Running in c8373ea3bb63
Removing intermediate container c8373ea3bb63
 ---> 49ea3cb6d9d9
Step 5/6 : FROM debian as final
 ---> c2c03a296d23
Step 6/6 : COPY --from=base bar.txt .
 ---> 76381cfb99f0
Successfully built 76381cfb99f0
Successfully tagged example:latest

$ docker run --rm example ls bar.txt
bar.txt

$ docker run --rm example ls foo.txt
ls: cannot access 'foo.txt': No such file or directory

$ docker history --format "{{ .CreatedBy }}" example --no-trunc
/bin/sh -c #(nop) COPY file:fcd11fceabc5279bf6c4356a288665bdd0a19391f4214976c3687e9ba5cb548a in .
/bin/sh -c #(nop)  CMD ["bash"]
/bin/sh -c #(nop) ADD file:770e381defc5e4a0ba5df52265a96494b9f5d94309234cb3f7bc6b00e1d18f9a in /

$ docker history --format "{{ .CreatedBy }}" 46837435cff7 --no-trunc
|1 MY_SECRET=foo /bin/sh -c echo $MY_SECRET > foo.txt
/bin/sh -c #(nop)  ARG MY_SECRET
/bin/sh -c #(nop)  CMD ["bash"]
/bin/sh -c #(nop) ADD file:770e381defc5e4a0ba5df52265a96494b9f5d94309234cb3f7bc6b00e1d18f9a in /
```

## BuildKit Secrets
For scenarios where you have secrets already stored on disk on the Docker host machine, such as a private RSA key, you can make use of a [feature](https://docs.docker.com/develop/develop-images/build_enhancements/#new-docker-build-secret-information) in Docker's BuildKit for passing files securely in a `docker build` command. This feature is currently only supported for building Linux containers. It avoids storing secrets in any of the built Docker layers, including when using multi-stage builds. But, of course, the file does exist on the Docker host machine and needs to be properly secured to prevent any unauthorized access.

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
DOCKER_BUILDKIT=1 docker build . --secret id=id_rsa,src=~/.ssh/id_rsa -t example --progress=plain
```

## Cleaning the Build Results
As mentioned previously, the act of building Docker images can leave secret data on the Docker host machine even if the image being published is free of any secrets. Depending on your build environment, this may or may not be an issue. If you've determined that you do need to clean the results of the build to ensure no secrets are left in any Docker artifacts, here are some commands that can be helpful:

* Remove any container using the image: `docker rm -f <CONTAINER NAME>`
  An image cannot be deleted if it's being used by any containers so you'll first need to make sure the containers are deleted. See the [documentation](https://docs.docker.com/engine/reference/commandline/rm/).
* Remove a specific image: `docker rmi -f <IMAGE ID>`
  Note that an image consists of intermediate layers, each having their own image ID. You need to delete all of them in order to completely delete all layers associated with the image. You can find the image IDs of an image's intermediate layers by running `docker history <IMAGE ID> --format "{{ .ID }}"`. See the [documentation](https://docs.docker.com/engine/reference/commandline/rmi/).
* Remove all unused data: `docker system prune -a -f`
  This is a heavy-handed way to delete all unused data in your Docker environment, including containers, networks, and images. See the [documentation](https://docs.docker.com/engine/reference/commandline/system_prune/).
  

