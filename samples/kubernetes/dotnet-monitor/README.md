# Resource-limited app

[Kubernetes](https://kubernetes.io/) enables limiting the CPU and/or memory resources that can be used by a container.

Run [resource-limits.yaml](resource-limits.yaml) on your cluster with the following command.

```bash
kubectl apply -f https://raw.githubusercontent.com/dotnet/dotnet-docker/main/samples/kubernetes/resource-limits/resource-limits.yaml
```

Apply the local file if you've cloned the repo.

```bash
kubectl apply -f resource-limits.yaml
```

See resource limits for the deployment.

```bash
kubectl describe deployment
```

Or look at the pod (only relevant output shown).

```bash
$ kubectl get po
NAME                                      READY   STATUS    RESTARTS   AGE
dotnet-resource-limits-7cb5b7f559-kgkdg   1/1     Running   0          12s
$ kubectl describe pod dotnet-resource-limits-7cb5b7f559-kgkdg
Name:             dotnet-resource-limits-7cb5b7f559-kgkdg
Containers:
  aspnetapp:
    Container ID:   docker://e5dc46f714c978322fb11daca6beb20d1dd17c879f077ccd1daad880b07432b7
    Image:          mcr.microsoft.com/dotnet/samples:aspnetapp
    Limits:
      cpu:     2
      memory:  100Mi
    Requests:
      cpu:        2
      memory:     60Mi
```

Create a proxy to the service.

```bash
kubectl port-forward service/dotnet-monitor 8080:80 52323
```

View the sample app at http://localhost:8080/ or call `curl http://localhost:8080/Environment`.

Resources can be deleted using the following pattern:

```bash
kubectrl delete -f https://raw.githubusercontent.com/dotnet/dotnet-docker/main/samples/kubernetes/resource-limits/resource-limits.yaml
```
