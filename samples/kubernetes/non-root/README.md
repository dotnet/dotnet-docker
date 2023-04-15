# Securing containers with a non-root user with Kubernetes

[Kubernetes](https://kubernetes.io/) provides a way to validate that a [non-root user](https://devblogs.microsoft.com/dotnet/securing-containers-with-rootless/) is used.

This sample uses .NET 8.

Apply [non-root.yaml](non-root.yaml) to your cluster.

```bash
kubectl apply -f https://raw.githubusercontent.com/dotnet/dotnet-docker/main/samples/kubernetes/non-root/non-root.yaml
```

Or use the manifest directly if you've cloned the repo.

```bash
kubectl apply -f non-root.yaml
```

You can validate the user with the following commands.

```bash
$kubectl get po
NAME                               READY   STATUS    RESTARTS   AGE
dotnet-non-root-7db6ff8b6d-zdc9n   1/1     Running   0          21s
$ kubectl exec dotnet-non-root-7db6ff8b6d-zdc9n -- whoami
app
```

Create a proxy to the service.

```bash
kubectl port-forward service/dotnet-non-root 8080
```

View the sample app at http://localhost:8080/ or call `curl http://localhost:8080/Environment`.

```bash
% curl http://localhost:8080/Environment
{"runtimeVersion":".NET 8.0.0-preview.3.23174.8","osVersion":"Linux 5.15.49-linuxkit #1 SMP PREEMPT Tue Sep 13 07:51:32 UTC 2022","osArchitecture":"Arm64","user":"app","processorCount":4,"totalAvailableMemoryBytes":4124512256,"memoryLimit":0,"memoryUsage":29655040}
```

`user` is displayed as `app`, as expected.

Delete the resources (remote URL or local manifest).

```bash
kubectl delete -f https://raw.githubusercontent.com/dotnet/dotnet-docker/main/samples/kubernetes/non-root/non-root.yaml
```

## SecurityContext

[`SecurityContext`](https://kubernetes.io/docs/reference/generated/kubernetes-api/v1.24/#securitycontext-v1-core) holds security configuration that will be applied to a container, in a Pod or [Deployment manifest](non-root.yaml). These fields can be used to configure and/or validate that the container image is running as non-root. This sample follows [Kubernetes "Restricted" hardening best practices](https://kubernetes.io/docs/concepts/security/pod-security-standards/#restricted).

This `securityContext` object enforces non-root hosting:

```yml
    spec:
      containers:
      - name: aspnetapp
        image: dotnetnonroot.azurecr.io/aspnetapp
        securityContext:
          allowPrivilegeEscalation: false
          runAsNonRoot: true
```

- [allowPrivilegeEscalation](https://kubernetes.io/docs/tasks/configure-pod-container/security-context/) -- Prevents (if `false`) a process from gaining greater privileges than its parent process. This is a good setting, but not directly related to users.
- `runAsNonRoot` -- Tests that the user (via uid) is a non-root user, otherwise fail.

The `USER` Dockerfile instruction must be set via `UID` for the the `runAsNonRoot` setting to work correctly, as demonstrated by [Dockerfile.alpine-non-root](https://github.com/dotnet/dotnet-docker/blob/f4786b8c0b4469f7eb18f891fd6c090561e50006/samples/aspnetapp/Dockerfile.alpine-non-root#L27) and the following example.

```dockerfile
USER $APP_UID
```
