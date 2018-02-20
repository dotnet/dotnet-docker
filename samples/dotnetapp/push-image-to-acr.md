# Push Docker Images to Azure Container Registry

This sample demonstrates hot to push .NET Core container images to [Azure Container Registry (ACR)](https://docs.microsoft.com/en-us/azure/container-registry/container-registry-get-started-portal). The instructions are based on the [.NET Core Docker Sample](README.md).

These instructions use the [Azure CLI](https://docs.microsoft.com/cli/azure/install-azure-cli) and the [Docker client](https://www.docker.com/products/docker).

The same instructions are also available to [push to Azure DockerHub](push-image-to-dockerhub.md).

## Create ACR Registry

The following example demonstrates how to create a private ACR Registry. Once an image is in ACR, it is very easy to deploy it to ACI.

The instructions use example values that need to be changed to for your environment, specifically the password location, the registry name and the user account. More simply, make sure to change "rich" and "richlander" to something else.

```console
az login
az group create --name richlander-containers --location westus
az acr create --name richlander --resource-group richlander-containers --sku Basic
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

There are a few ways to login with these credentials, some of which are demonstrated below.

The easiest approach is to get the credentials and login in a single command, piping the `az acr credential` command to `docker login`. This approach works on Windows, macOS and Linux.

```console
az acr credential show -n richlander --query passwords[0].value --output tsv | docker login richlander.azurecr.io -u richlander --password-stdin
```

Alternatively, if you want to persist the password across logins, you can do the following. Make sure to save to a location not managed by source control (to avoid accidental disclosure).

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

## Build the Image

The following instructions are a subset of the [dotnetapp sample](dotnetapp/README.md) instruction, assuming that you are starting from the root of the [dotnet-docker repo](https://github.com/dotnet/dotnet-docker).

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

Now tag and push the image to ACR. You can also build the image with the right name initially. It's harder to write instructions that way.

```console
docker tag dotnetapp richlander.azurecr.io/dotnetapp
docker push richlander.azurecr.io/dotnetapp
```

## Pull the Image from Another Device

You can pull the image from another device. You need to `docker login` to ACR before you can pull the image, just like was described previously.

You need to update the path locations, registry, and user names to the ones you are using.

You can use one of the same techniques shown earlier or logon with a clear password on the commandline, as shown in the following command.

```console
docker login richlander.azurecr.io -u richlander --password thepassword
```

Now pull and run the image:

```console
docker run --rm richlander.azurecr.io/dotnetapp
```
