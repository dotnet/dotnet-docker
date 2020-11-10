# Push Docker Images to Docker Hub

This sample demonstrates hot to push .NET images to the [Docker Hub](https://hub.docker.com/) container registry. The instructions are based on the [.NET Docker Sample](README.md).

Similar instructions are also available to [push to Azure Container Registry](push-image-to-acr.md).

## Build the Image

The following instructions are a subset of the [.NET Docker Sample](README.md) instructions, which assume that you are starting from the root of the [dotnet-docker repo](https://github.com/dotnet/dotnet-docker).

```console
cd samples
cd dotnetapp
docker build --pull -t dotnetapp .
```

You can test the image with the following instructions.

```console
docker run --rm dotnetapp
```

## Tag the Image

Now tag the image to push to Docker Hub, with your Docker Hub user name. You can also build the image with the right name initially.

```console
docker tag dotnetapp richlander/dotnetapp
```

## Login to Docker Hub

You need to [login to Docker Hub](https://docs.docker.com/docker-hub/accounts/) with [`docker login`](https://docs.docker.com/engine/reference/commandline/login/) or with the Docker Client UI to push images.

There are a couple ways of passing a password to `docker login`. These instructions [pass the password to stdin](https://github.com/docker/cli/pull/218) via a text file called password-dh.txt. Make sure to save to a location not managed by source control (to avoid accidental disclosure).

> Note: The instructions use example values that need to be changed to for your environment, specifically the password location, and the user account. More simply, make sure to change "rich" and "richlander" to something else.

Login on Windows:

```console
type c:\users\rich\password-dh.txt | docker login -u richlander --password-stdin
```

Login on macOS or Linux:

```console
cat ~/password-dh.txt | docker login -u richlander --password-stdin
```

Alternatively, pass your password to `docker login` as plain text via the `--password` argument.

## Push the Image

Now push the image to Docker Hub.

```console
docker push richlander/dotnetapp
```

## Pull the Image from Another Device

You can now pull the image from another device. You need to `docker login` if you are using a private Docker Hub repo using the same login instructions used previously.

Update the path locations, registry, and user names to the ones you are using.

Now pull and run the image:

```console
docker pull richlander/dotnetapp
docker run --rm richlander/dotnetapp
```

## More Samples

* [.NET Docker Samples](../README.md)
* [.NET Framework Docker Samples](https://github.com/microsoft/dotnet-framework-docker-samples/)
