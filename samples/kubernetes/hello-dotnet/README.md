#  app

[Kubernetes](https://kubernetes.io/) offers a `LoadBalancer` service type that is intended for exposing a service to the internet. This sample is intended for testing on a service like Azure Kubernetes Service (AKS).

Run [hello-cloud-dotnet.yaml](hello-cloud-dotnet.yaml) on your cluster with the following command.

```bash
kubectl apply -f https://raw.githubusercontent.com/dotnet/dotnet-docker/main/samples/kubernetes/hello-cloud-dotnet/hello-cloud-dotnet.yaml
```

Or use the manifest directly if you've cloned the repo.

```bash
kubectl apply -f hello-cloud-dotnet.yaml
```

To run on AKS or another cloud service, configure `kubectl` to access that kubernetes cluster control plane. Otherwise, you can configure your local environment to [create a `LoadBalancer` tunnel](https://minikube.sigs.k8s.io/docs/handbook/accessing/#example-of-loadbalancer).

You can [discover the external IP](https://learn.microsoft.com/en-us/azure/aks/tutorial-kubernetes-deploy-application?tabs=azure-cli#test-the-application) for the service using `kubectl`.

```bash
% kubectl get service -w
NAME                 TYPE           CLUSTER-IP    EXTERNAL-IP   PORT(S)          AGE
hello-cloud-dotnet   LoadBalancer   10.0.186.62   <pending>     8080:32751/TCP   3s
kubernetes           ClusterIP      10.0.0.1      <none>        443/TCP          100m
hello-cloud-dotnet   LoadBalancer   10.0.186.62   20.237.122.134   8080:32751/TCP   9s
```

This sample is configured to use port `8080`.

You can delete the resources with the following pattern (remote URL or local manifest).

```bash
kubectl delete -f https://raw.githubusercontent.com/dotnet/dotnet-docker/main/samples/kubernetes/hello-cloud-dotnet/hello-cloud-dotnet.yaml
```
