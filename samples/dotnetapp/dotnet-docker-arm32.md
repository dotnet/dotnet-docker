# Build .NET Core Applications for ARM32 and Raspberry Pi with Docker

You can build and run .NET Core apps with [Docker for Raspberry Pi and ARM32 devices](https://docs.docker.com/install/linux/docker-ce/debian), generally. These instructions are based on the [.NET Core Docker Sample](README.md).

> Note: that Docker refers to ARM32 as `armhf` in documentation and other places.

## Building the Sample with Docker

Build the [sample](Dockerfile.debian-arm32) on a 64-bit operating system. This requirement is due to the .NET Core SDK not being currently supported on ARM32. The instructions assume that you are in the root of the repository.

```console
cd samples
cd dotnetapp
docker build --pull -t dotnetapp:debian-arm32 -f Dockerfile.debian-arm32 .
```

See [Build .NET Core Self-Contained Applications with Docker](dotnet-docker-selfcontained.md) to build a self-contained .NET Core ARM32 application.

Multiple variations of this sample have been provided, as follows. Some of these example Dockerfiles are demonstrated later. Specify an alternate Dockerfile via the `-f` argument.

* [Debian ARM32 sample with build and unit testing](Dockerfile.debian-arm32)
* [Debian self-contained ARM32 sample with build and unit testing](Dockerfile.debian-arm32-selfcontained)

## Pushing the image to a Container Registry

Push the image to a container registry after building the image so that you can pull it from an ARM32 device. Instructions are provided for pushing to both Azure Container Registry and DockerHub (you only need to choose one):

* [Push Docker Images to Azure Container Registry](push-image-to-acr.md)
* [Push Docker Images to DockerHub](push-image-to-dockerhub.md)

## Pull the Image from Another Device

Next, pull the image from the registry you pushed your image to, on an ARM32 device (like a Pi).

> Note: The instructions use example values that need to be changed to for your environment, specifically the password location, and the user account. More simply, make sure to change "rich" and "richlander" to something else.

### Using Azure Container Registry (ACR)

Now pull and run the image from Azure Container Registry if you used that registry:

```console
docker pull richlander.azurecr.io/dotnetapp:debian-arm32
docker run --rm richlander.azurecr.io/dotnetapp:debian-arm32
```

First `docker login` to Azure Container Registry. See [Push Docker Images to Azure Container Registry](push-image-to-acr.md) for instructions on how to do that.

### Using DockerHub

Now pull and run the image from DockerHub if you used that registry:

```console
docker pull richlander/dotnetapp:debian-arm32
docker run --rm richlander/dotnetapp:debian-arm32
```
