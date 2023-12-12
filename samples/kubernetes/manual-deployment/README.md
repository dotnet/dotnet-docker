# Manual deployment

[kubectl](https://kubernetes.io/docs/reference/kubectl/) enables manual deployment of resources (without manifests).

You can do that with a few quick commands

```bash
kubectl create deployment dotnet-app --image mcr.microsoft.com/dotnet/samples:aspnetapp
kubectl expose deployment dotnet-app --type=ClusterIP --port=8080
```

View the resources that have been deployed.

```bash
kubectl get pod
kubectl get service
kubectl get deployment
```

If the container takes a while to download and launch, you can add `-w` to `get pod` to watch updates.

Create a proxy to the service.

```bash
kubectl port-forward service/dotnet-app 8080:8080
```

View the sample app at http://localhost:8080/ or call `curl http://localhost:8080/Environment`.

Delete the resources.

```bash
kubectl delete service dotnet-app
kubectl delete deployment dotnet-app
```
