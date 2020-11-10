# Deploy ASP.NET Core Applications to Azure Container Instances

You can deploy ASP.NET Core applications to Azure Container Instances (ACI) with Docker. ACI is a great option for application testing and can also be used for production deployment (not covered here). These instructions are based on the [ASP.NET Core Docker Sample](README.md).

These instructions work for both Linux and Windows based images.  Be aware that ACI does not support all versions of Windows images.  See [What Windows base OS images are supported](https://docs.microsoft.com/en-us/azure/container-instances/container-instances-faq#what-windows-base-os-images-are-supported) for details.

## Build Application

Build the application per the [ASP.NET Core Docker Sample](README.md) instructions. The following is a summarized version of those instructions. The instructions assume that you are in the root of the repository.

```console
cd samples
cd aspnetapp
docker build --pull -t aspnetapp -f Dockerfile .
```

## Create ACR Registry

Create an ACR registry per the instructions at [Push Docker Images to Azure Container Registry](push-image-to-acr.md). The following is a summarized version of those instructions.

> Note: Change the password location and the user account ("rich" and "richlander") example values in your environment.

```console
az login
az group create --name richlander-containers --location westus
az acr create --name richlander --resource-group richlander-containers --sku Basic
```

## Login to Azure Container Registry

First, "admin-enable" your session, an ACR credentials access prerequisite for the subsequent command.

```console
az acr update -n richlander --admin-enabled true
```

Now login to ACR via the docker cli, an ACR push prerequisite:

```console
az acr credential show -n richlander --query passwords[0].value --output tsv | docker login richlander.azurecr.io -u richlander --password-stdin
```

## Push Image for Azure Container Registry (ACR)

Use the following instructions to tag the image for your registry and push the image. If you automate these instructions, build the image with the correct name initially.

```console
docker tag aspnetapp richlander.azurecr.io/aspnetapp
docker push richlander.azurecr.io/aspnetapp
```

## Deploy Image to Azure Container Instance (ACI)

During deployment, you'll need to enter your password. Type or copy/paste it in. Get your password beforehand from the following command:

```console
az acr credential show -n richlander --query passwords[0].value --output tsv
```

You can deploy images with the following command:

### Linux

```console
az container create --os-type Linux --name aspnetapp --image richlander.azurecr.io/aspnetapp --resource-group richlander-containers --ip-address public
```

### Windows

```console
az container create --os-type Windows --name aspnetapp --image richlander.azurecr.io/aspnetapp --resource-group richlander-containers --ip-address public
```

## Running the Image

The previous step -- `az container create` -- will create and start the container.  Once completed, detailed status information will be displayed.  Verify the `provisioningState` is `Succeeded`, collect the IP address from the `ip` field, as you can see in the following image, and then copy/paste the IP address into your browser. You should see the sample running.

![az container show -- successfully provisioned app](https://user-images.githubusercontent.com/2608468/29669868-b492c4e8-8899-11e7-82cc-d3ae1262a080.png)

## Cleanup

When these containers aren't needed, delete the resource group to reclaim all exercise container resources.

```console
az group delete --name richlander-containers
az group exists --name richlander-containers
```

## More Samples

* [.NET Docker Samples](../README.md)
* [.NET Framework Docker Samples](https://github.com/microsoft/dotnet-framework-docker-samples/)
