# Replicas backed by health-checks

Use multiple replicas and health checks for reliability.

```bash
kubectl apply -f https://raw.githubusercontent.com/dotnet/dotnet-docker/main/kubernetes/replicas-and-health/replica-health.yaml
```

Apply the local file if you've cloned the repo.

```bash
kubectl apply -f replica-health.yaml
```

Start viewing pods.

```bash
kubectl get -w pod
```

In another terminal, create a proxy to the service.

```bash
kubectl port-forward service/hello-dotnet 8080:80
```

View the sample app at http://localhost:8080/ or call `curl http://localhost:8080/Environment`.

Resources can be deleted using the following pattern:

```bash
kubectrl delete -f https://raw.githubusercontent.com/dotnet/dotnet-docker/main/kubernetes/replicas-and-health/replica-health.yaml
```
