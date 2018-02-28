# Deploy ASP.NET Core Applications to Azure Container Instances

You can deploy ASP.NET Core applications to Azure Container Instances (ACI) with Docker. ACI is a great option for application testing and can also be use for production deployment (not covered here). These instructions are based on the [ASP.NET Core Docker Sample](README.md) sample.

## Build Application

Build the application per the [ASP.NET Core Docker Sample](README.md) instructions. The following is a summarized version of those instructions. The instructions assume that you are in the root of the repository.

```console
cd samples
cd aspnetapp
docker build --pull -t aspnetapp -f Dockerfile .
```

## Create ACR Registry

Create an ACR registry per the instructions at [Push Docker Images to Azure Container Registry](../dotnetapp/push-image-to-acr.md). The following is a summarized version of those instructions.

The instructions use example values that need to be changed for your environment, specifically the resource group name, and the registry name. More simply, make sure to change "richlander" to something else.

```console
az login
az group create --name richlander-containers --location westus
az acr create --name richlander --resource-group richlander-containers --sku Basic
```

## Get Registry Credentials

Use the following instructions to get credentials for your registry, in order to push to it.

```console
az acr update -n richlander --admin-enabled true
az acr credential show -n richlander
```

## Login to Azure Container Registry

Use the following instructions to login to ACR.

```console
az acr credential show -n richlander | docker login richlander.azurecr.io -u richlander --password-stdin
```

## Push Image for Azure Container Registry (ACR)

Use the following instructions to tag the image for your registry and push the image. If you automate these instructions, build the image with the correct name initially.

```console
docker tag aspnetapp richlander.azurecr.io/aspnetapp
docker push richlander.azurecr.io/aspnetapp
```

## Deploy Image to Azure Container Instance (ACI)

```console
az container create --name aspnetapp --image richlander.azurecr.io/aspnetapp --resource-group richlander-containers --ip-address public
```

Specify `--os-type Windows` for Windows images. Windows Server, version 1709 are not yet supported.

You will be asked for your password. Write or paste it in.

## Running the Image

The last step -- `az container show` -- will need to be repeated until `provisioningState` moves to `Succeeded`.

```console
az container show --name aspnetapp --resource-group richlander-containers
```

Once the `provisioningState` moves to `Succeeded`, collect the IP address from the `ip` field, as you can see in the following image, and then copy/paste the IP address into your browser. You should see the sample running.

![az container show -- successfully provisioned app](https://user-images.githubusercontent.com/2608468/29669868-b492c4e8-8899-11e7-82cc-d3ae1262a080.png)

## Cleanup

After you no longer want to use these containers, delete the resource group to reclaim all container resources from this experiment.

```console
az group delete --name richlander-containers
az group exists --name richlander-containers
```
