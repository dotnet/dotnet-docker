# Deploy ASP.NET Core Applications to Azure Container Instances

You can deploy ASP.NET Core applications to Azure Container Instances (ACI) with Docker. ACI is a great option for application testing and can also be use for production deployment (not covered here). These instructions are based on the [ASP.NET Core Docker Sample](README.md) sample.

## Build Application

Build the application per the [ASP.NET Core Docker Sample](README.md) instructions. The following is a summarized version of those instructions. The instructions assume that you are in the root of the repository.

```console
cd samples
cd aspnetapp
docker build -t aspnetapp -f Dockerfile .
```

## Create ACR Registry

You need a place to push your image. You can use [Azure Container Registry](../dotnetapp/push-image-to-acr.md) or [DockerHub](../dotnetapp/push-image-to-dockerhub.md). The following example demonstrates how to create a private ACR Registry. Once an image is in ACR, it is very easy to deploy it to ACI.

```
az login
az group create --name richlander-containers --location westus
az acr create --name richlander --resource-group richlander-containers --sku Basic
```

## Get Registry Credentials

You need to get credentials for the Registry in order to know how to push to it.

```console
az acr update -n richlander --admin-enabled true
```

## Login to Azure Container Registry

Complete instructions are provided for login at [Push Docker Images to Azure Container Registry](../dotnetapp/push-image-to-acr.md). The following is a summarized version of those instructions.

These instructions pass the password to stdin via a text file called password-acr.txt. They use example values that need to be changed for your environment, specifically the password location, the registry name and the user account. More simply, make sure to change "rich" and "richlander" to something else.

Login on Windows:

```console
type c:\users\rich\password-acr.txt | docker login richlander.azurecr.io -u richlander --password-stdin
```

Login on macOS or Linux:

```console
cat ~/password-acr.txt | docker login richlander.azurecr.io -u richlander --password-stdin
```

## Push Image for Azure Container Registry (ACR)

You need to do the following to push an image to ACR:

* Login to ACR.
* Tag the image to push to ACR.
* Push image to ACR.

Instructions are provided for these tasks at [Push Docker Images to Azure Container Registry](../dotnetapp/push-image-to-acr.md).

Now tag and push the image to ACR. You can also build the image with the right name initially. It's harder to write instructions that way.

```console
docker tag aspnetapp richlander.azurecr.io/aspnetapp
docker push richlander.azurecr.io/aspnetapp
```

You need to [login](https://docs.microsoft.com/azure/container-registry/container-registry-get-started-portal#log-in-to-acr) to ACR with [`docker login`](https://docs.docker.com/engine/reference/commandline/login/) to push images. ACR registries are private, so `pull`, `push`, and any other registry operation requires login.


You can deploy your ASP.NET Core application to [Azure Container Instances](https://azure.microsoft.com/en-us/blog/announcing-azure-container-instances/) with just a few commands. You can use the following instructions or use the [Azure Container Instances Quickstart](https://docs.microsoft.com/azure/container-instances/container-instances-quickstart)

These instructions require:

* [Azure CLI](https://docs.microsoft.com/en-us/cli/azure/install-azure-cli) installed,which is supported on Windows, macOS and Linux.
* That you have run the commands above, and that you have an `aspnetapp` image created.
* A docker registry account, such as [Docker Hub](https://hub.docker.com/). Change "richlander" in the following example to your account.

```console
docker tag aspnetapp richlander/aspnetapp
docker push richlander/aspnetapp
az group create --name TestACIGroup --location eastus
az container create --name aspnetapp --image richlander/aspnetapp --resource-group TestACIGroup --ip-address public
az container show --name aspnetapp --resource-group TestACIGroup
```

The last step -- `az container show` -- will need to be repeated until `provisioningState` moves to `Succeeded`. At that point, collect the IP address from the `ip` field, as you can see in the following image, and then copy/paste the IP address into your browser. You should see the sample running.

![az container show -- successfully provisioned app](https://user-images.githubusercontent.com/2608468/29669868-b492c4e8-8899-11e7-82cc-d3ae1262a080.png)

You will then want to delete your container and double check that no containers are left running in the resource group.

```console
az container delete --name aspnetapp --resource-group TestACIGroup
az container list --resource-group TestACIGroup
```
