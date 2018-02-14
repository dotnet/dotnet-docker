# Push Docker Images to Azure Container Registry

You can build and push .NET Core container images to [Azure Container Registry (ACR)](https://docs.microsoft.com/en-us/azure/container-registry/container-registry-get-started-portal). These instructions help you do that and are based on the [.NET Core Docker Sample](README.md).

These instructions use the [Azure CLI](https://docs.microsoft.com/cli/azure/install-azure-cli) and the [Docker client](https://www.docker.com/products/docker).

## Create ACR Registry

The following example demonstrates how to create a private ACR Registry. Once an image is in ACR, it is very easy to deploy it to ACI.

The instructions use example values that need to be changed to for your environment, specifically the password location, the registry name and the user account. More simply, make sure to change "rich" and "richlander" to something else.

```console
az login
az group create --name richlander-containers --location westus
az acr create --name richlander --resource-group richlander-containers --sku Basic
```

## Get Registry Credentials

You need to get credentials for the Registry in order to know how to push to it.

```console
az acr update -n richlander --admin-enabled true
```

Get credentials on Windows:

```console
az acr credential show -n richlander --query passwords[0].value --output tsv > %USERPROFILE%\password-acr.txt
```

Get credentials on macOS and Linux:

```console
az acr credential show -n richlander --query passwords[0].value --output tsv > ~\password-acr.txt
```

## Login to ACR

You need to [login](https://docs.microsoft.com/azure/container-registry/container-registry-get-started-portal#log-in-to-acr) to ACR with [`docker login`](https://docs.docker.com/engine/reference/commandline/login/) to push images. ACR registries are private, so `pull`, `push`, and any other registry operation requires login.

There are a couple ways of passing a password to `docker login`. These instructions pass the password to stdin via a text file called password-acr.txt. It is recommended to put the password file in a location outside of a source control location (I put it in the root of my user profile). This approach is considered a best practice and the most secure.

The instructions use example values that need to be changed to for your environment, specifically the password location, the registry name and the user account. More simply, make sure to change "rich" and "richlander" to something else.

Login on Windows:

```console
type %USERPROFILE%\password-acr.txt | docker login richlander.azurecr.io -u richlander --password-stdin
```

Login on macOS or Linux:

```console
cat ~/password-acr.txt | docker login richlander.azurecr.io -u richlander --password-stdin
```

## Build the Image

The instructions in following sections assume that you built an image per the instructions at [dotnetapp sample](dotnetapp/README.md). You can also build an image with your own instructions, too.

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

Next, you can pull the image from another device. You need to `docker login` to ACR before you can pull the image, just like was described previously.

You need to update the path locations, registry, and user names to the ones you are using.

Login on Windows:

```console
type c:\users\rich\password-acr.txt | docker login richlander.azurecr.io -u richlander --password-stdin
```

Login on macOS or Linux:

```console
cat ~/password-acr.txt | docker login richlander.azurecr.io -u richlander --password-stdin
```

Now pull and run the image:

```console
docker run --rm richlander.azurecr.io/dotnetapp
```
