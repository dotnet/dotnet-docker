# Configure a .NET app with Kubernetes

This .NET app sample is fully-configured (for a simple app) with respect to [Kubernetes](https://kubernetes.io/) settings. It includes both `ClusterIP` and `LoadBalancer` variants, for local and cloud cluster use, respectively.

This sample uses/requires a non-root user, has container limits set, uses multiple replicas, and registers a liveness probe.

## Run on your local cluster

Apply to your local cluster, using a `ClusterIP` service.

```bash
$ kubectl apply -f https://raw.githubusercontent.com/dotnet/dotnet-docker/main/samples/kubernetes/hello-dotnet/hello-dotnet.yaml
$ kubectl port-forward service/hello-dotnet 8080
Forwarding from 127.0.0.1:8080 -> 8080
Forwarding from [::1]:8080 -> 8080
```

View the sample app at http://localhost:8080/ and call `curl http://localhost:8080/Environment`.

You can also apply the `LoadBalancer` variant locally. However, you may find that it [doesn't work as you might expect locally](https://stackoverflow.com/questions/59412733/kubernetes-docker-desktop-with-multiple-loadbalancer-services) if you use Docker Desktop, Minikube or the like. It is more intended for use with a real cluster.

### Using Docker Desktop

Deploy the `LoadBalancer` manifest.

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

Note the difference in port. Port `80` is being used, as defined in the deployment manifest.

### Using minikube

First, install and/or switch to [minikube](https://minikube.sigs.k8s.io/docs/), if neccessary.

```bash
$ kubectl config get-contexts
CURRENT   NAME             CLUSTER          AUTHINFO         NAMESPACE
*         docker-desktop   docker-desktop   docker-desktop   
          minikube         minikube         minikube         default
$ kubectl config use-context minikube
Switched to context "minikube".
$ kubectl config get-contexts        
CURRENT   NAME             CLUSTER          AUTHINFO         NAMESPACE
          docker-desktop   docker-desktop   docker-desktop   
*         minikube         minikube         minikube         default
```

Deploy the app the same way.

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
curl http://localhost/Environment
{"runtimeVersion":".NET 8.0.4","osVersion":"Ubuntu 22.04.4 LTS","osArchitecture":"Arm64","user":"app","processorCount":1,"totalAvailableMemoryBytes":78643200,"memoryLimit":104857600,"memoryUsage":42151936,"hostName":"hello-dotnet-86f4cffb9d-blcnb"}
```

This is the same as with Docker Desktop, just with the additional requirement of using `minikube tunnel`.

## Run on your cloud cluster

To run on Azure Kubernetes Service (AKS) or another cloud service, configure `kubectl` to access that kubernetes cluster control plane. For AKS, you can do that [via the Azure CLI](https://learn.microsoft.com/azure/aks/learn/quick-kubernetes-deploy-cli#connect-to-the-cluster). This same command is available via the "Connect" menu in the Azure Portal (for an AKS resource).

```bash
$ az aks get-credentials --resource-group myResourceGroup --name myAKSCluster
$ kubectl get nodes
NAME                                STATUS   ROLES   AGE   VERSION
aks-agentpool-81348477-vmss000004   Ready    agent   2d    v1.26.0
```

Apply the `LoadBalancer1 deployment to that cluster.

```bash
$ kubectl apply -f https://raw.githubusercontent.com/dotnet/dotnet-docker/main/samples/kubernetes/hello-dotnet/hello-dotnet-loadbalancer.yaml
$ kubectl get service -w
NAME           TYPE           CLUSTER-IP     EXTERNAL-IP   PORT(S)        AGE
hello-dotnet   LoadBalancer   10.0.120.232   <pending>     80:31567/TCP   5s
kubernetes     ClusterIP      10.0.0.1       <none>        443/TCP        42h
hello-dotnet   LoadBalancer   10.0.120.232   20.51.80.224   80:31567/TCP   9s
```

The `EXTERNAL-IP` will eventually resolve and you will be able to access it in the same way, via `curl` or the browser, on port `80`.
