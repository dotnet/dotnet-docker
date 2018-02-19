# Build .NET Core Applications for Raspberry Pi with Docker

You can build and run .NET Core apps with [Docker for Raspberry Pi and ARM32 devices](https://docs.docker.com/install/linux/docker-ce/debian), generally. These instructions are based on the [.NET Core Docker Sample](README.md).

> Note: that Docker refers to ARM32 as `armhf` in documentation and other places.

## Building the Sample with Docker

You need to build the [sample](Dockerfile.linux-arm32) on a 64-bit operating system. This requirement is due to the .NET Core SDK not being currently supported on ARM32. The instructions assume that you are in the root of the repository.

```console
cd samples
cd dotnetapp
docker build --pull -t dotnetapp -f Dockerfile.linux-arm32 .
```

## Pushing the image to a Container Registry

You need to push the image to a container registry after building the image so that you can pull it from an ARM32 device. Instructions are provided for pushing to both Azure Container Registry and DockerHub (you only need to choose one):

* [Push Docker Images to Azure Container Registry](push-image-to-acr.md)
* [Push Docker Images to DockerHub](push-image-to-dockerhub.md)

## Pull the Image from Another Device

You will next want to pull the image from an ARM32 device (like a Pi). 

Update the path locations, registry, and user names to the ones you are using.

### Using Azure Container Registry (ACR)

Now pull and run the image from ACR if you used that registry.

Login for Azure Container Registry with `docker login`:

```console
docker login richlander.azurecr.io -u richlander --password thepassword
```

Now pull and run the image:

```console
docker run --rm richlander.azurecr.io/dotnetapp
```

### Using DockerHub

Now pull and run the image from DockerHub if you used that registry:

```console
docker run --rm richlander/dotnetapp
```
