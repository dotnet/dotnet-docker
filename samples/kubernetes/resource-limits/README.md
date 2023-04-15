# Limiting container resources with Kubernetes

[Kubernetes](https://kubernetes.io/) enables [limiting the CPU and/or memory resources](https://kubernetes.io/docs/concepts/configuration/manage-resources-containers/) that can be used by a container.

Apply [resource-limits.yaml](resource-limits.yaml) to your cluster.

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
dotnet-resource-limits-54b5c75fdd-jjcbp   1/1     Running   0          12s
$ kubectl describe pod dotnet-resource-limits-54b5c75fdd-jjcbp
    Limits:
      cpu:     500m
      memory:  100Mi
    Requests:
      cpu:        250m
      memory:     60Mi
```

Create a proxy to the service.

```bash
kubectl port-forward service/dotnet-resource-limits 8080
```

View the sample app at http://localhost:8080/ or call `curl http://localhost:8080/Environment`. You can see memory and CPU usage displayed. CPU is reported as an integer and always rounded up to the next whole integer (for example, `0.25` is rounded up to `1`).

```bash
% curl http://localhost:8080/Environment
{"runtimeVersion":".NET 7.0.5","osVersion":"Linux 5.15.49-linuxkit #1 SMP PREEMPT Tue Sep 13 07:51:32 UTC 2022","osArchitecture":"Arm64","user":"root","processorCount":1,"totalAvailableMemoryBytes":78643200,"memoryLimit":104857600,"memoryUsage":31354880}
```

`processorCount` is display as `1`, as expected.

Delete the resources (remote URL or local manifest).

```bash
kubectrl delete -f https://raw.githubusercontent.com/dotnet/dotnet-docker/main/samples/kubernetes/resource-limits/resource-limits.yaml
```
