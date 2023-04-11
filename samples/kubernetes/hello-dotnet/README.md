# Configure a .NET app with Kubernetes

This .NET app sample is fully-configured (for a simple app) with respect to [Kubernetes](https://kubernetes.io/) settings. It includes both `ClusterIP` and `LoadBalancer` variants, for local and cloud cluster use, respectively.

This sample is configured to use port `8080`, uses a non-root user, and is running on .NET 8.

## Run on your local cluster

Apply to your local cluster, using a `ClusterIP` service.

```bash
$ kubectl apply -f https://raw.githubusercontent.com/dotnet/dotnet-docker/main/samples/kubernetes/hello-dotnet/hello-dotnet.yaml
$ kubectl port-forward service/hello-dotnet 8080:80
```

View the sample app at http://localhost:8080/ and call `curl http://localhost:8080/Environment`.

## Run on your cloud cluster

Apply to your local cluster, using a `LoadBalancer` service.

```bash
$ kubectl apply -f https://raw.githubusercontent.com/dotnet/dotnet-docker/main/samples/kubernetes/hello-dotnet/hello-dotnet-loadbalancer.yaml
$ kubectl get service -w
```

You can discover the external IP for the service using `kubectl`.

```bash
% kubectl get service -w
NAME                 TYPE           CLUSTER-IP    EXTERNAL-IP   PORT(S)          AGE
hello-cloud-dotnet   LoadBalancer   10.0.186.62   <pending>     8080:32751/TCP   3s
kubernetes           ClusterIP      10.0.0.1      <none>        443/TCP          100m
hello-cloud-dotnet   LoadBalancer   10.0.186.62   20.237.122.134   8080:32751/TCP   9s
```

Otherwise, you can configure your local environment to [create a `LoadBalancer` tunnel](https://minikube.sigs.k8s.io/docs/handbook/accessing/#example-of-loadbalancer), per whichever local cluster software you are using.

View the sample app at http://EXTERNAL-IP:8080/ and call `curl http://EXTERNAL-IP:8080/Environment`.

### Configure `kubectl` to access your cloud service

To run on Azure Kubernetes Service (AKS) or another cloud service, configure `kubectl` to access that kubernetes cluster control plane. For AKS, you can do that [via the Azure CLI](https://learn.microsoft.com/azure/aks/learn/quick-kubernetes-deploy-cli#connect-to-the-cluster). This same command is available via the "Connect" menu in the Azure Portal (for an AKS resource).

```bash
$ az aks get-credentials --resource-group myResourceGroup --name myAKSCluster
$ kubectl get nodes
NAME                                STATUS   ROLES   AGE   VERSION
aks-agentpool-81348477-vmss000004   Ready    agent   2d    v1.26.0
```
