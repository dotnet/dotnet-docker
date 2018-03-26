# Build and run the sample for Linux ARM32 with Docker

You can build and run .NET Core apps with [Docker for Raspberry Pi and ARM32 devices](https://docs.docker.com/install/linux/docker-ce/debian), generally. These instructions are based on the [ASP.NET Core Docker Sample](README.md).

> Note: that Docker refers to ARM32 as `armhf` in documentation and other places.

## Building the Sample with Docker

Build the [sample](Dockerfile.debian-arm32) on a 64-bit operating system. This requirement is due to the .NET Core SDK not being currently supported on ARM32. The instructions assume that you are in the root of the repository.

```console
cd samples
cd aspnetapp
docker build --pull -t aspnetapp:debian-arm32 -f Dockerfile.debian-arm32 .
```

## Pushing the image to a Container Registry

Push the image to a container registry after building the image so that you can pull it from an ARM32 device. Instructions are provided for pushing to both Azure Container Registry and DockerHub (you only need to choose one):

* [Push Docker Images to Azure Container Registry](../dotnetapp/push-image-to-acr.md)
* [Push Docker Images to DockerHub](../dotnetapp/push-image-to-dockerhub.md)

## Pull the Image from Another Device

Next, pull the image from the registry you pushed your image to, on an ARM32 device (like a Pi).

> Note: The instructions use example values that need to be changed to for your environment, specifically the password location, and the user account. More simply, make sure to change "rich" and "richlander" to something else.

### Using Azure Container Registry (ACR)

Now pull and run the image from Azure Container Registry if you used that registry:

```console
docker pull richlander.azurecr.io/aspnetapp:debian-arm32
docker run --rm -p 8000:80 richlander.azurecr.io/aspnetapp:debian-arm32
```

First `docker login` to Azure Container Registry. See [Push Docker Images to Azure Container Registry](../dotnetapp/push-image-to-acr.md) for instructions on how to do that.

### Using DockerHub

Now pull and run the image from DockerHub if you used that registry:

```console
docker pull richlander/aspnetapp:debian-arm32
docker run --rm -p 8000:80 richlander/aspnetapp:debian-arm32
```

### Viewing the Site

After the application starts, visit the site one of two ways:

* From the web browser on the ARM32 device at `http://localhost:8000`
* From the web browser on another device on the same network on the ARM32 device IP on port 8000, similar to: `http://192.168.1.18:8000`

## More Samples

* [.NET Core Docker Samples](../README.md)
* [.NET Framework Docker Samples](https://github.com/microsoft/dotnet-framework-docker-samples/)
