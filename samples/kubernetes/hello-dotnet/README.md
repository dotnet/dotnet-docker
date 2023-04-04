# Single node app

Host a [sample app](../../samples/aspnetapp/Dockerfile.alpine) by applying a sample manifest (deployment and service).

Launch an app on your cluster with the following command.

```bash
kubectl apply -f https://raw.githubusercontent.com/dotnet/dotnet-docker/main/samples/kubernetes/hello-dotnet/hello-dotnet.yaml
```

Or use the manifest directly if you've cloned the repo.

```bash
kubectl apply -f hello-dotnet.yaml
```

Alternatively, you can do the same with a few quick commands

```bash
kubectl create deployment hello-dotnet --image mcr.microsoft.com/dotnet/samples:aspnetapp
kubectl expose deployment hello-dotnet --type=NodePort --port=80
```

Create a proxy to the service.

```bash
kubectl port-forward service/hello-dotnet 8080:80
```

View the sample app at http://localhost:8080/ or call `curl http://localhost:8080/Environment`.

View the active resources that have been deployed and then delete them.

```bash
kubectl get pod
kubectl get service
kubectl get deployment
kubectl delete service hello-dotnet
kubectl delete deployment hello-dotnet
```

Alternatively, you can delete the resources with the following pattern (remote URL or local manifest).

```bash
kubectl delete -f https://raw.githubusercontent.com/dotnet/dotnet-docker/main/samples/kubernetes/hello-dotnet/hello-dotnet.yaml
```
