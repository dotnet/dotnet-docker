# Resource-limited app

Limit memory and CPU for an app with a [sample app](../../samples/aspnetapp/Dockerfile.alpine).

```bash
kubectl apply -f https://raw.githubusercontent.com/dotnet/dotnet-docker/main/kubernetes/resource-limits/resource-limits.yaml
```

Apply the local file if you've cloned the repo.

```bash
kubectl apply -f resource-limits.yaml
```

See resource limits for the deployment.

```bash
kubectl describe deployment
```

Create a proxy to the service.

```bash
kubectl port-forward service/dotnet-resource-limits 8080:80
```

View the sample app at http://localhost:8080/ or call `curl http://localhost:8080/Environment`.

Resources can be deleted using the following pattern:

```bash
kubectrl delete -f https://raw.githubusercontent.com/dotnet/dotnet-docker/main/kubernetes/resource-limits/resource-limits.yaml
```
