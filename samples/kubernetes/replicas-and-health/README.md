# Configuring pod replicas and health checks with Kubernetes

[Kubernetes](https://kubernetes.io/) enables creating multiple [replicas](https://kubernetes.io/docs/concepts/workloads/controllers/replicaset/) and registering [health checks](https://kubernetes.io/docs/tasks/configure-pod-container/configure-liveness-readiness-startup-probes/).

There are multiple forms of health checks. The sample demonstrates a `liveness` check. The following warning is copied from [Kubernetes docs](https://kubernetes.io/docs/tasks/configure-pod-container/configure-liveness-readiness-startup-probes/).

> Caution: Liveness probes can be a powerful way to recover from application failures, but they should be used with caution. Liveness probes must be configured carefully to ensure that they truly indicate unrecoverable application failure, for example a deadlock.

Apply [replica-health.yaml](replica-health.yaml) to your cluster.

```bash
kubectl apply -f https://raw.githubusercontent.com/dotnet/dotnet-docker/main/samples/kubernetes/replicas-and-health/replica-health.yaml
```

Apply the local file if you've cloned the repo.

```bash
kubectl apply -f replica-health.yaml
```

Look at the replicas created.

```bash
% kubectl get po
NAME                                     READY   STATUS    RESTARTS   AGE
dotnet-replica-health-64d49554c9-f8mlp   1/1     Running   0          8s
dotnet-replica-health-64d49554c9-fz5ms   1/1     Running   0          8s
dotnet-replica-health-64d49554c9-m6cfr   1/1     Running   0          8s
```

There should be three.

Look at the events for one of the replicas.

```bash
% kubectl describe pod dotnet-replica-health-64d49554c9-f8mlp
Name:             dotnet-replica-health-64d49554c9-f8mlp
Namespace:        default
Events:
  Type    Reason     Age   From               Message
  ----    ------     ----  ----               -------
  Normal  Scheduled  31s   default-scheduler  Successfully assigned default/dotnet-replica-health-64d49554c9-f8mlp to minikube
  Normal  Pulled     31s   kubelet            Container image "mcr.microsoft.com/dotnet/samples:aspnetapp" already present on machine
  Normal  Created    31s   kubelet            Created container aspnetapp
  Normal  Started    30s   kubelet            Started container aspnetapp
```

Much of the output has been removed in this example. The key part is the events. Kubernetes will show a failing container, as is demonstrated in [Configure Liveness, Readiness and Startup Probes](https://kubernetes.io/docs/tasks/configure-pod-container/configure-liveness-readiness-startup-probes/), as follows.

```bash
 Type     Reason     Age                From               Message
  ----     ------     ----               ----               -------
  Normal   Scheduled  57s                default-scheduler  Successfully assigned default/liveness-exec to node01
  Normal   Pulling    55s                kubelet, node01    Pulling image "registry.k8s.io/busybox"
  Normal   Pulled     53s                kubelet, node01    Successfully pulled image "registry.k8s.io/busybox"
  Normal   Created    53s                kubelet, node01    Created container liveness
  Normal   Started    53s                kubelet, node01    Started container liveness
  Warning  Unhealthy  10s (x3 over 20s)  kubelet, node01    Liveness probe failed: cat: can't open '/tmp/healthy': No such file or directory
  Normal   Killing    10s                kubelet, node01    Container liveness failed liveness probe, will be restarted
```

It is the `Unhealthy` Reason that is used when a liveness probe fails. The container will be killed after that.

You can call the health check manually with the following pattern.

```bash
$ kubectl exec dotnet-replica-health-64d49554c9-f8mlp -- wget -qO- -t1 http://localhost:80/healthz
Healthy
```

Create a proxy to the service.

```bash
kubectl port-forward service/dotnet-replica-health 8080
```

View the sample app at http://localhost:8080/ or call `curl http://localhost:8080/Environment`.

Delete the resources (remote URL or local manifest).

```bash
kubectrl delete -f https://raw.githubusercontent.com/dotnet/dotnet-docker/main/samples/kubernetes/replicas-and-health/replica-health.yaml
```
