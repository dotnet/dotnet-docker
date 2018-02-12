# Push Docker Images to Dockerhub

You can build and push .NET Core container images to the [DockerHub](https://hub.docker.com/) container registry. These instructions help you do that and are based on the [dotnetapp sample](README.md).

## Login to ACR

You need to [login](https://docs.microsoft.com/azure/container-registry/container-registry-get-started-portal#log-in-to-acr) to DockerHub with [`docker login`](https://docs.docker.com/engine/reference/commandline/login/) or with the Docker Client UI to push images.

There are a couple ways of passing a password to `docker login`. These instructions pass the password to stdin via a text file called password-acr.txt. It is recommended to put the password file in a location outside of a source control location (I put it in the root of my user profile). This approach is considered a best practice and the most secure.

The instructions use example values that need to be changed to for your environment, specifically the password location, and the user account. More simply, make sure to change "rich" and "richlander" to something else.

Login on Windows:

```console
type c:\users\rich\password-dh.txt | docker login -u richlander --password-stdin
```

Login on macOS or Linux:

```console
cat ~/password-dh.txt | docker login -u richlander --password-stdin
```

## Build the Image

The instructions in following sections assume that you built an image per the instructions at [dotnetapp sample](dotnetapp/README.md). You can also build an image with your own instructions, too.

The following instructions are a subset of the [dotnetapp sample](dotnetapp/README.md) instruction, assuming that you are starting from the root of the [dotnet-docker repo](https://github.com/dotnet/dotnet-docker).

```console
cd samples
cd dotnetapp
docker build -t dotnetapp .
```

You can test the image with the following instructions.

```console
docker run dotnetapp
```

## Push the Image

Now tag and push the image to DockerHub. You can also build the image with the right name initially. It's just harder to write instructions that way.

```console
docker tag dotnetapp richlander/dotnetapp
docker push richlander/dotnetapp
```

## Pull the Image from Another Device

You can pull the image from another device. You need to `docker login` to DockerHub (or login via the client UI) before you can pull the image, just like was described previously.

You need to update the path locations, registry, and user names to the ones you are using.

Login on Windows:

```console
type c:\users\rich\password-dh.txt | docker login -u richlander --password-stdin
```

Login on macOS or Linux:

```console
cat ~/password-dh.txt | docker login -u richlander --password-stdin
```

Now pull and run the image:

```console
docker run --rm richlander/dotnetapp
```

For fun, you might try [running an .NET Core image on Raspberry Pi](dotnet-docker-arm32.md). Specific instructions have been provided for that scenario.
