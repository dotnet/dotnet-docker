# Best Practices for Managing NuGet Credentials in Docker Scenarios

There are often cases when your Dockerfile needs to build a .NET project that make use of a NuGet package located in a private feed. How do you properly provide your Docker build access to that private feed in a secure manner? The snippets below provide recommended approaches you can take.

## Preventing Leakage of the Credentials

The main aspect that we need to concern ourselves with in all of these approaches is to prevent leakage of the credentials to consumers of the published Docker image. It can be easy to do if you're not careful. Let's first go over some Docker concepts and then we'll see how their usage can lead to leakage of sensitive data.

### Docker Layers

The concept of Docker layers is much too complex of a topic to discuss here so please read the [Docker documentation on layers](https://docs.docker.com/storage/storagedriver/) if you're not already familiar with them. The key thing to understand about a Docker image is that all of the layers from which it is composed are available to be peeked at. If a file was added in one layer and then deleted in a subsequent layer, that file may not exist in the resulting container layer but it still exists in the layer that added the file. And each of an image's layers are accessible to consumers of the image.

#### Example

This example shows a file being added in one layer and then being removed in a subsequent layer. The resulting container layer does not contain the file but, as is shown, the file is still accessible from the image's layers.

```
# Dockerfile
FROM debian

RUN echo secret > secret.txt
RUN rm secret.txt
```

```
$ docker build . -t example

$ docker run --rm example ls secret.txt
ls: cannot access 'secret.txt': No such file or directory

$ docker save example -o example.tar

$ tar tvf example.tar
drwxr-xr-x  0 root   root        0 Oct 09 11:27 010189bc11d3df6eeb155fefc1e1af116df4d6b532accb3ae94d9b4261b8edfc/
-rw-r--r--  0 root   root        3 Oct 09 11:27 010189bc11d3df6eeb155fefc1e1af116df4d6b532accb3ae94d9b4261b8edfc/VERSION
-rw-r--r--  0 root   root     1206 Oct 09 11:27 010189bc11d3df6eeb155fefc1e1af116df4d6b532accb3ae94d9b4261b8edfc/json
-rw-r--r--  0 root   root     1536 Oct 09 11:27 010189bc11d3df6eeb155fefc1e1af116df4d6b532accb3ae94d9b4261b8edfc/layer.tar
drwxr-xr-x  0 root   root        0 Oct 09 11:27 1070caa1a8d89440829fd35d9356143a9d6185fe7f7a015b992ec1d8aa81c78a/
-rw-r--r--  0 root   root        3 Oct 09 11:27 1070caa1a8d89440829fd35d9356143a9d6185fe7f7a015b992ec1d8aa81c78a/VERSION
-rw-r--r--  0 root   root      401 Oct 09 11:27 1070caa1a8d89440829fd35d9356143a9d6185fe7f7a015b992ec1d8aa81c78a/json
-rw-r--r--  0 root   root 119199744 Oct 09 11:27 1070caa1a8d89440829fd35d9356143a9d6185fe7f7a015b992ec1d8aa81c78a/layer.tar
-rw-r--r--  0 root   root      1776 Oct 09 11:27 175ff829656a84d0d904d6184a7ea083b034dd78ac990802f9304646f81b363f.json
drwxr-xr-x  0 root   root         0 Oct 09 11:27 539b8293941b5f2648d90c94e0076a3e8f9b274dd260c511321d029700a9be70/
-rw-r--r--  0 root   root         3 Oct 09 11:27 539b8293941b5f2648d90c94e0076a3e8f9b274dd260c511321d029700a9be70/VERSION
-rw-r--r--  0 root   root       477 Oct 09 11:27 539b8293941b5f2648d90c94e0076a3e8f9b274dd260c511321d029700a9be70/json
-rw-r--r--  0 root   root      2048 Oct 09 11:27 539b8293941b5f2648d90c94e0076a3e8f9b274dd260c511321d029700a9be70/layer.tar
-rw-r--r--  0 root   root       354 Dec 31  1969 manifest.json
-rw-r--r--  0 root   root        87 Dec 31  1969 repositories

$ tar xf example.tar 539b8293941b5f2648d90c94e0076a3e8f9b274dd260c511321d029700a9be70/layer.tar

$ tar tvf 539b8293941b5f2648d90c94e0076a3e8f9b274dd260c511321d029700a9be70\layer.tar
-rw-r--r--  0 root   root        4 Oct 09 11:27 secret.txt

$ tar xf 539b8293941b5f2648d90c94e0076a3e8f9b274dd260c511321d029700a9be70\layer.tar secret.txt

$ echo secret.txt
secret
```

### ARG Instruction

The [`ARG` instruction](https://docs.docker.com/engine/reference/builder/#arg) provides a way to pass in values that the Dockerfile can consume at build time. The important thing to realize about `ARG` values is that they can be exposed in the `docker history` by the layer that consumes them.

#### Example

In this example, an `ARG` is passed a secret value and then used by a `RUN` command. The fact that the secret is stored in a file is irrelevant; all that matters is that the `MYSECRET` environment variable is referenced in the `RUN` command. Because it is referenced, the value shows up in `docker history`.

```
# Dockerfile
FROM debian
ARG MY_SECRET
RUN echo $MY_SECRET > secret.txt
```

```
$ docker build . --build-arg MY_SECRET=foo -t example

$ docker history --format "{{ .CreatedBy }}" example --no-trunc
|1 MY_SECRET=foo /bin/sh -c echo $MY_SECRET > secret.txt
/bin/sh -c #(nop)  ARG MY_SECRET
/bin/sh -c #(nop)  CMD ["bash"]
/bin/sh -c #(nop) ADD file:770e381defc5e4a0ba5df52265a96494b9f5d94309234cb3f7bc6b00e1d18f9a in /
```

### Multi-Stage Builds

The next concept builds upon the idea of Docker layers. [Multi-stage builds](https://docs.docker.com/develop/develop-images/multistage-build/) are a mechanism for authoring Dockerfiles in multiple stages with each stage having its own base image and instructions. Files from an earlier stage can be explicitly copied to a later stage. The canonical scenario of multi-stage builds is a stage that builds the application and a final stage that runs the application. _Only the layers produced by the final stage end up in the resulting image._ That's an important thing to keep in mind when thinking about ways to avoid having secrets leak out.

#### Example

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

$ docker run --rm example ls bar.txt
bar.txt

$ docker run --rm example ls foo.txt
ls: cannot access 'foo.txt': No such file or directory

$ docker history --format "{{ .CreatedBy }}" example --no-trunc
/bin/sh -c #(nop) COPY file:fcd11fceabc5279bf6c4356a288665bdd0a19391f4214976c3687e9ba5cb548a in .
/bin/sh -c #(nop)  CMD ["bash"]
/bin/sh -c #(nop) ADD file:770e381defc5e4a0ba5df52265a96494b9f5d94309234cb3f7bc6b00e1d18f9a in /
```

## Best Practice: Restore from Feeds Using Basic Authentication

If you use basic authentication to connect to your private NuGet feed, this option applies to you. This pattern involves using the [`ARG` instruction](https://docs.docker.com/engine/reference/builder/#arg) to pass in the NuGet feed credentials to a `docker build` command. Care must be taken here to ensure that the `ARG` instructions are only used for intermediate stages and never the final stage to avoid the leaking the credentials in the final image.

For this scenario, we're going to rely on a `nuget.config` file that makes use of NuGet's support for [environment variables](https://docs.microsoft.com/en-us/nuget/reference/nuget-config-file#using-environment-variables):

```
<?xml version="1.0" encoding="UTF-8"?>
<configuration>
  <packageSources>
    <add key="public" value="https://api.nuget.org/v3/index.json" />
    <add key="customfeed" value="https://mycustomfeedurl"  />
  </packageSources>

  <packageSourceCredentials>
    <customfeed>
      <add key="Username" value="%Nuget_CustomFeedUserName%" />
      <add key="ClearTextPassword" value="%Nuget_CustomFeedPassword%" />
    </customfeed>
  </packageSourceCredentials>
</configuration>
```

When making use of the `ARG` instruction the name of the argument is made available as an environment variable during the scope of the `docker build`. This aligns nicely with our use of environment variables in the `nuget.config`.

In the Dockerfile below, there are two stages: build and runtime. The build stage is responsible for building the application project. It defines two `ARG` values which match the names of the environment variables used in the the `nuget.config`. But the build stage is not the final stage of the Dockerfile so the `--build-arg` values that are passed to the `docker build` command do not get exposed in the resulting image. All the final stage is really doing is copying the built output of the application from the build stage since that's all that's needed to run the application.

```
FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS build

ARG Nuget_CustomFeedUserName
ARG Nuget_CustomFeedPassword

WORKDIR /app

# Copy csproj and restore as distinct layers
COPY *.csproj .
COPY ./nuget.config .
RUN dotnet restore

# Copy and publish app and libraries
COPY . .
RUN dotnet publish -c Release -o out  --no-restore


FROM mcr.microsoft.com/dotnet/core/runtime:3.0 AS runtime
WORKDIR /app/
COPY --from=build /app/out ./
ENTRYPOINT ["dotnet", "dotnetapp.dll"]
```

This Dockerfile would be built using this command:

```
$ docker build . --build-arg Nuget_CustomFeedUserName=<username> --build-arg Nuget_CustomFeedPassword=<password>
```

Passing the username and password values to the `docker build` command in this manner can be useful in automated scenarios when those values are stored as environment variables.

## Best Practice: Using the Azure Artifact Credential Provider

NuGet has support for [custom credential providers](https://docs.microsoft.com/en-us/nuget/reference/extensibility/nuget-exe-credential-providers) that provide a mechanism to authenticate to feeds both interactively and non-interactively. One such implementation of this is [Azure Artifact Credential Provider](https://github.com/Microsoft/artifacts-credprovider) which provides a mechanism to acquire credentials for restoring NuGet packages from Azure Artifacts feeds. In unattended build agent scenarios, like building Docker images, a `VSS_NUGET_EXTERNAL_FEED_ENDPOINTS` [environment variable](https://github.com/Microsoft/artifacts-credprovider#environment-variables) is used to supply an access token.

If you're using Azure Artifacts for your private NuGet feeds, this scenario is for you since it provides a compatible experience between interactive developer machines and automated scenarios like Docker image builds.

In this scenario, we'll use the Credential Provider to provide credentials for a private NuGet feed. The `VSS_NUGET_EXTERNAL_FEED_ENDPOINTS` environment variable is meant to be an extension of the `nuget.config` file, allowing both to be used together with the `VSS_NUGET_EXTERNAL_FEED_ENDPOINTS` environment variable simply supplying the access token. So the `nuget.config` used here does not define any `packageSourceCredentials` element for `customfeed`:

```
<?xml version="1.0" encoding="UTF-8"?>
<configuration>
  <packageSources>
    <add key="public" value="https://api.nuget.org/v3/index.json" />
    <add key="customfeed" value="https://mycustomfeedurl"  />
  </packageSources>
</configuration>
```

Instead, the credentials for `customfeed` are defined in the Dockerfile by making use of an `ARG` for the access token:

```
FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS build
WORKDIR /app

ARG FEED_ACCESSTOKEN
ENV VSS_NUGET_EXTERNAL_FEED_ENDPOINTS \
    "{\"endpointCredentials\": [{\"endpoint\":\"https://thalman.pkgs.visualstudio.com/_packaging/test/nuget/v3/index.json\", \"username\":\"docker\", \"password\":\"${FEED_ACCESSTOKEN}\"}]}"

RUN curl -L https://raw.githubusercontent.com/Microsoft/artifacts-credprovider/master/helpers/installcredprovider.sh  | bash

# Copy csproj and restore as distinct layers
COPY *.csproj .
COPY ./nuget.config .
RUN dotnet restore

# Copy and publish app and libraries
COPY . .
RUN dotnet publish -c Release -o out --no-restore


FROM mcr.microsoft.com/dotnet/core/runtime:3.0 AS runtime
WORKDIR /app/
COPY --from=build /app/out ./
ENTRYPOINT ["dotnet", "dotnetapp.dll"]
```

_Note that a script is called to install the Credential Provider. When `dotnet restore` is run, the Credential Provider is invoked to resolve the credentials and it retrieves them from the `VSS_NUGET_EXTERNAL_FEED_ENDPOINTS` environment variable._

This Dockerfile would be built using this command:

```
$ docker build . --build-arg FEED_ACCESSTOKEN=<access_token>
```

Passing the access token to the `docker build` command in this manner can be useful in automated scenarios when that value is stored as an environment variable.

## Best Practice: Passing Secrets by File with Docker BuildKit

Docker BuildKit provides some enhanced functionality for passing [secret information](https://docs.docker.com/develop/develop-images/build_enhancements/#new-docker-build-secret-information) when running `docker build`. This avoids the use of `ARG` values which could potentially be exposed in the resulting image unless you're disciplined in defining your multi-stage build. But it also requires that the secret data be stored in a file on the Docker host machine during the `docker build`. If your `nuget.config` contain credentials and is already stored on the Docker host machine, then this may be a good option for you. (Be careful when storing unprotected credentials on disk. Make sure that machine and file are properly secured.)

Let's use the following sample `nuget.config` which is stored on the Docker host machine:

```
<?xml version="1.0" encoding="UTF-8"?>
<configuration>
  <packageSources>
    <add key="public" value="https://api.nuget.org/v3/index.json" />
    <add key="customfeed" value="https://mycustomfeedurl"  />
  </packageSources>

  <packageSourceCredentials>
    <customfeed>
      <add key="Username" value="<username>" />
      <add key="ClearTextPassword" value="<password>" />
    </customfeed>
  </packageSourceCredentials>
</configuration>
```

The Dockerfile needs to make use of a special syntax to describe the secret it's attempting to access. The Dockerfile must enable the new syntax features by overriding the default frontend (see the [documentation](https://docs.docker.com/develop/develop-images/build_enhancements/#overriding-default-frontends) for more info). Next, it uses the `--mount-type-secret` [syntax](https://docs.docker.com/develop/develop-images/build_enhancements/#new-docker-build-secret-information) to access the secret from the `/run/secrets/` location.

```
# syntax = docker/dockerfile:1.0-experimental

FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS build
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY *.csproj .
RUN --mount=type=secret,id=nugetconfig \
    dotnet restore --configfile /run/secrets/nugetconfig
```

When building this Dockerfile, BuildKit must be enabled and the `--secret` option is used to provide the path to the `nuget.config` file:

```
$ DOCKER_BUILDKIT=1 docker build . --secret id=nugetconfig,src=nuget.config
```
