# Configure a .NET app with Kubernetes

This .NET app sample demonstrates multiple [Kubernetes](https://kubernetes.io/) settings. It uses/requires a non-root user, has container limits set, uses multiple replicas, and registers a liveness probe.

The following instructions demonstrate how to apply the `ClusterIP` and `LoadBalancer` variants of the `hello-dotnet` sample. `ClusterIP` is the default service type and works well for local clusters and private cloud deployments. `LoadBalancer` is intended for exposing an app to the internet. They are other service types, such as `Ingress`.

## Run on your local cluster

Apply to your local cluster, using a `ClusterIP` service.

```bash
$ kubectl apply -f https://raw.githubusercontent.com/dotnet/dotnet-docker/main/samples/kubernetes/hello-dotnet/hello-dotnet.yaml
$ kubectl port-forward service/hello-dotnet 8080
Forwarding from 127.0.0.1:8080 -> 8080
Forwarding from [::1]:8080 -> 8080
```

View the sample app at http://localhost:8080/ and call `curl http://localhost:8080/Environment`.

```bash
$ curl http://localhost:8080/Environment 
{"runtimeVersion":".NET 8.0.4","osVersion":"Ubuntu 22.04.4 LTS","osArchitecture":"Arm64","user":"app","processorCount":1,"totalAvailableMemoryBytes":78643200,"memoryLimit":104857600,"memoryUsage":31797248,"hostName":"hello-dotnet-5b887d9fbd-b6bmh"}
```

The `LoadBalancer` variant can also be applied locally. It will use port `80`.

The following example uses Docker Desktop.

```bash
$ kubectl apply -f hello-dotnet-loadbalancer.yaml 
deployment.apps/hello-dotnet created
service/hello-dotnet created
$ kubectl get po
NAME                           READY   STATUS    RESTARTS   AGE
hello-dotnet-bd5fdfcfb-f5vjq   1/1     Running   0          4s
hello-dotnet-bd5fdfcfb-nmshh   1/1     Running   0          4s
hello-dotnet-bd5fdfcfb-twp9d   1/1     Running   0          4s
$ kubectl get service
NAME           TYPE           CLUSTER-IP      EXTERNAL-IP   PORT(S)        AGE
hello-dotnet   LoadBalancer   10.101.23.131   localhost     80:31466/TCP   12s
kubernetes     ClusterIP      10.96.0.1       <none>        443/TCP        29h
```

And then call the `Environment` endpoint with `curl`.

```bash
$ curl http://localhost/Environment 
{"runtimeVersion":".NET 8.0.4","osVersion":"Ubuntu 22.04.4 LTS","osArchitecture":"Arm64","user":"app","processorCount":1,"totalAvailableMemoryBytes":78643200,"memoryLimit":104857600,"memoryUsage":54845440,"hostName":"hello-dotnet-bd5fdfcfb-f5vjq"}
```

The following example uses minikube.

```bash
$ kubectl apply -f hello-dotnet-loadbalancer.yaml 
deployment.apps/hello-dotnet created
service/hello-dotnet created
$ kubectl get svc -w
NAME           TYPE           CLUSTER-IP     EXTERNAL-IP   PORT(S)        AGE
hello-dotnet   LoadBalancer   10.96.208.21   <pending>     80:32597/TCP   29s
kubernetes     ClusterIP      10.96.0.1      <none>        443/TCP        2m28s
```

The `EXTERNAL-IP` will stay `pending` forever.

In another terminal, run [`minikube tunnel`](https://minikube.sigs.k8s.io/docs/handbook/accessing/#example-of-loadbalancer).

```bash
$ minikube tunnel  
✅  Tunnel successfully started
```

As soon as that is run, the `EXTERNAL-IP` will be provided, back in the original terminal.

```bash
$ kubectl get svc -w
NAME           TYPE           CLUSTER-IP     EXTERNAL-IP   PORT(S)        AGE
hello-dotnet   LoadBalancer   10.96.208.21   <pending>     80:32597/TCP   29s
kubernetes     ClusterIP      10.96.0.1      <none>        443/TCP        2m28s
hello-dotnet   LoadBalancer   10.96.208.21   127.0.0.1     80:32597/TCP   2m9s
```

And then a call with `curl`.

```bash
$ curl http://localhost/Environment
{"runtimeVersion":".NET 8.0.4","osVersion":"Ubuntu 22.04.4 LTS","osArchitecture":"Arm64","user":"app","processorCount":1,"totalAvailableMemoryBytes":78643200,"memoryLimit":104857600,"memoryUsage":42151936,"hostName":"hello-dotnet-86f4cffb9d-blcnb"}
```

This is the same as with Docker Desktop, just with the additional requirement of using `minikube tunnel`.

## Run on your cloud cluster

Apply to your local cluster, using a `LoadBalancer` service.

The example uses Azure Kubernetes Service (AKS) as the [Kubernetes environment](../environment.md)

```bash
$ az aks get-credentials --resource-group myResourceGroup --name myAKSCluster
$ kubectl get nodes                  
NAME                                STATUS   ROLES    AGE   VERSION
aks-agentpool-17841206-vmss000000   Ready    <none>   31m   v1.29.2
aks-agentpool-17841206-vmss000001   Ready    <none>   32m   v1.29.2
aks-userpool-17841206-vmss000000    Ready    <none>   32m   v1.29.2
aks-userpool-17841206-vmss000001    Ready    <none>   32m   v1.29.2
```

Apply the `LoadBalancer` deployment to that cluster.

```bash
$ kubectl apply -f https://raw.githubusercontent.com/dotnet/dotnet-docker/main/samples/kubernetes/hello-dotnet/hello-dotnet-loadbalancer.yaml
$ kubectl get service -w
NAME           TYPE           CLUSTER-IP     EXTERNAL-IP   PORT(S)        AGE
hello-dotnet   LoadBalancer   10.0.120.232   <pending>     80:31567/TCP   5s
kubernetes     ClusterIP      10.0.0.1       <none>        443/TCP        42h
hello-dotnet   LoadBalancer   10.0.120.232   20.51.80.224   80:31567/TCP   9s
```

The `EXTERNAL-IP` will resolve (after a brief wait) and you will be able to access it in the same way, via `curl` or the browser, on port `80`.

```bash
$ curl http://20.51.80.224/Environment
{"runtimeVersion":".NET 8.0.4","osVersion":"Ubuntu 22.04.4 LTS","osArchitecture":"X64","user":"app","processorCount":1,"totalAvailableMemoryBytes":78643200,"memoryLimit":104857600,"memoryUsage":42323968,"hostName":"hello-dotnet-bd5fdfcfb-twgqs"}
```

You may notice that this approach is using raw HTTP on an internet-facing endpoint. That's only acceptable for brief testing like this. [Enforce HTTPS in ASP.NET Core](https://learn.microsoft.com/aspnet/core/security/enforcing-ssl) and [What is Application Gateway Ingress Controller?](https://learn.microsoft.com/azure/application-gateway/ingress-controller-overview) describe options for exposing secure HTTPS endpoints.
