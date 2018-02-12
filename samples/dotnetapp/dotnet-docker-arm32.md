# Build .NET Core Applications for Raspberry Pi with Docker

You can build and run .NET Core apps with [Docker for Raspberry Pi and ARM32 devices](https://docs.docker.com/install/linux/docker-ce/debian),generally. These instructions are based on the [dotnetapp](README.md) sample.

> Note: that Docker refers to ARM32 as `armhf` in documentation and other places.

## Building the Sample with Docker

You need to build the [sample](Dockerfile.arm32) on a 64-bit operating system, either Windows (in Linux container mode), macOS or Linux. This is because the .NET Core SDK is currently not supported on ARM32. The instructions assume that you are in the root of the repository.

```console
cd samples
cd dotnetapp
docker build -t dotnetapp -f Dockerfile.arm32 .
```

## Pushing the image to a Container Registry

After building the image, you need to push the image to a container registry so that you can pull it from an ARM32 device. Straightforward instructions are provided for pushing to both Azure Container Registry and DockerHub (you only need to choose one).

* [Push Docker Images to Azure Container Registry](push-image-to-acr.md)
* [Push Docker Images to DockerHub](push-docker-image-to-dockerhub.md)

## Pull the Image from Another Device

Next, pull the image from an ARM32 device. You will need to `docker login` before you can pull the image, just like was described above.

Again, you will want to update the path locations, registry and user names to the ones you are using.

You only need to login to the registry you used for pushing your image.

### Using Azure Container Registry

Login for Azure Container Registry:

```console
cat ~/password-acr.txt | docker login richlander.azurecr.io -u richlander --password-stdin
```

Now pull and run the image:

```console
docker run --rm richlander.azurecr.io/dotnetapp
```

### Using DockerHub

Login for DockerHub:

```console
cat ~/password-dh.txt | docker login -u richlander --password-stdin
```

Now pull and run the image:

```console
docker run --rm richlander/dotnetapp
```
