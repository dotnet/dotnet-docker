# Push Docker Images to Azure Container Registry

This sample demonstrates how to push .NET images to [Azure Container Registry (ACR)](https://docs.microsoft.com/azure/container-registry/container-registry-get-started-portal). The instructions are based on the [.NET Docker Sample](README.md).

These instructions use the [Azure CLI](https://docs.microsoft.com/cli/azure/install-azure-cli) and the [Docker client](https://www.docker.com/products/docker).

Similar instructions are also available to [push to DockerHub](push-image-to-dockerhub.md).

## Build the Image

The following instructions are a subset of the [dotnetapp sample](dotnetapp/README.md) instruction, assuming that you are starting from the root of the [dotnet-docker repo](https://github.com/dotnet/dotnet-docker).

```console
cd samples
cd dotnetapp
docker build --pull -t dotnetapp .
```

You can test the image with the following instructions.

```console
docker run --rm dotnetapp
```

## Create ACR Registry

The following example demonstrates how to create a private ACR Registry. Once an image is in ACR, it is easy to deploy it to ACI.

> Note: The instructions use example values that need to be changed to for your environment, specifically the password location, and the user account. More simply, make sure to change "rich" and "richlander" to something else.

```console
az login
az group create --name richlander-containers --location westus
az acr create --name richlander --resource-group richlander-containers --sku Basic
```

## Tag the Image

Now tag the image to push to your ACR registry. You can also build the image with the right name initially.

```console
docker tag dotnetapp richlander.azurecr.io/dotnetapp
```

## Login to ACR

You need to [login to ACR](https://docs.microsoft.com/azure/container-registry/container-registry-get-started-portal#log-in-to-acr) with [`docker login`](https://docs.docker.com/engine/reference/commandline/login/) to push images. ACR registries are private, so `pull`, `push`, and any other registry operation requires login.

To interfact with your credentials, you need to first request to make admin calls to your account with the following command:

```console
az acr update -n richlander --admin-enabled true
```

You can see your credentials using the following command.

```console
az acr credential show -n richlander
```

There are a few ways to login with these credentials, some of which are demonstrated in the following examples.

The easiest approach is to get the credentials and login in a single command, piping the result of the `az acr credential` command to `docker login` [via stdin](https://github.com/docker/cli/pull/218). This approach works on Windows, macOS, and Linux.

```console
az acr credential show -n richlander --query passwords[0].value --output tsv | docker login richlander.azurecr.io -u richlander --password-stdin
```

Alternatively, you can persist your password across logins with the following technique. Make sure to save to a location not managed by source control (to avoid accidental disclosure).

Login on Windows:

```console
az acr credential show -n richlander --query passwords[0].value --output tsv > %USERPROFILE%\password-acr.txt
type %USERPROFILE%\password-acr.txt | docker login richlander.azurecr.io -u richlander --password-stdin
```

Login on macOS or Linux:

```console
az acr credential show -n richlander --query passwords[0].value --output tsv > ~\password-acr.txt
cat ~\password-acr.txt | docker login richlander.azurecr.io -u richlander --password-stdin
```

## Push the Image

Now push the image to your ACR registry.

```console
docker push richlander.azurecr.io/dotnetapp
```

## Pull the Image from Another Device

First, `docker login` to ACR before you can pull the image from another device, just like was described previously. Alternatively, you can pass your password to `docker login` as plain text via the `--password` argument.

Update the path locations, registry, and user names to the ones you are using.

Now pull and run the image (the first command isn't strictly necessary):

```console
docker pull richlander.azurecr.io/dotnetapp
docker run --rm richlander.azurecr.io/dotnetapp
```

## More Samples

* [.NET Docker Samples](../README.md)
* [.NET Framework Docker Samples](https://github.com/microsoft/dotnet-framework-docker-samples/)
