# Push Docker Images to Dockerhub

This sample demonstrates hot to push .NET Core container images to the [DockerHub](https://hub.docker.com/) container registry. The instructions are based on the [.NET Core Docker Sample](README.md).

The same instructions are also available to [push to Azure Container Registry](push-image-to-acr.md).

## Login to DockerHub

You need to [login to DockerHub](https://docs.docker.com/docker-hub/accounts/) with [`docker login`](https://docs.docker.com/engine/reference/commandline/login/) or with the Docker Client UI to push images.

There are a couple ways of passing a password to `docker login`. These instructions pass the password to stdin via a text file called password-acr.txt. Make sure to save to a location not managed by source control (to avoid accidental disclosure).

> Note: The instructions use example values that need to be changed to for your environment, specifically the password location, and the user account. More simply, make sure to change "rich" and "richlander" to something else.

Login on Windows:

```console
type c:\users\rich\password-dh.txt | docker login -u richlander --password-stdin
```

Login on macOS or Linux:

```console
cat ~/password-dh.txt | docker login -u richlander --password-stdin
```

Alternatively, you can pass your password to `docker login` as plain text as you can see in the following command:

```console
docker login -u richlander --password mypassword
```

## Build the Image

The following instructions are a subset of the [.NET Core Docker Sample](dotnetapp/README.md) instructions, which assume that you are starting from the root of the [dotnet-docker repo](https://github.com/dotnet/dotnet-docker).

```console
cd samples
cd dotnetapp
docker build --pull -t dotnetapp .
```

You can test the image with the following instructions.

```console
docker run dotnetapp
```

## Push the Image

Now tag and push the image to DockerHub. You can also build the image with the right name initially.

```console
docker tag dotnetapp richlander/dotnetapp
docker push richlander/dotnetapp
```

## Login to Registry

The instructions in this section are only required if you are using a private DockerHub repository.

You need to `docker login` to DockerHub (or login via the client UI) before you can pull the image.

You need to update the path locations, registry, and user names to the ones you are using.

Login on Windows:

```console
type c:\users\rich\password-dh.txt | docker login -u richlander --password-stdin
```

Login on macOS or Linux:

```console
cat ~/password-dh.txt | docker login -u richlander --password-stdin
```

## Pull the Image from Another Device

You can pull the image from another device. You will need to login to DockerHub if you are using a private repo.

You need to update the path locations, registry, and user names to the ones you are using.

Now pull and run the image:

```console
docker run --rm richlander/dotnetapp
```
