# Securing containers with a non-root user with Kubernetes

[Kubernetes](https://kubernetes.io/) provides a way to validate that a [non-root user](https://devblogs.microsoft.com/dotnet/securing-containers-with-rootless/) is used.

Apply [non-root.yaml](non-root.yaml) to your cluster.

```bash
kubectl apply -f https://raw.githubusercontent.com/dotnet/dotnet-docker/main/samples/kubernetes/non-root/non-root.yaml
```

Or use the manifest directly if you've cloned the repo.

```bash
kubectl apply -f non-root.yaml
```

You can validate the pod was correctly deployed. If you see `Running`, that's good.

```bash
$ kubectl get po
NAME                               READY   STATUS    RESTARTS   AGE
dotnet-non-root-7db6ff8b6d-zdc9n   1/1     Running   0          21s
```

You can also validate that `runAsNonRoot` was set.

```bash
$ kubectl get pod dotnet-non-root-7db6ff8b6d-zdc9n -o jsonpath="{.spec.securityContext}" 
{"runAsNonRoot":true}
```

In many cases, `kubectl exec` can be used to determine the user, like the following.

```bash
$kubectl get po
NAME                               READY   STATUS    RESTARTS   AGE
dotnet-fakepod-7db6ff8b6d-zdc9n   1/1     Running   0          21s
$ kubectl exec dotnet-fakepod-7db6ff8b6d-zdc9n -- whoami
app
```

This pattern won't work for this sample because `whoami` doesn't exist in .NET chiseled images. They are appliance images with no shell and no extra tools (like `whoami`, `top`, and `ps`). This pattern will work fine with apps built with our regular images (like `8.0`, `8.0-alpine` and `8.0-jammy`).

If you see `CreateContainerConfigError`, then the pod failed to deploy.

```bash
$ kubectl get po                 
NAME                               READY   STATUS                       RESTARTS   AGE
dotnet-non-root-775d58594f-6w959   0/1     CreateContainerConfigError   0          3s
```

In this example, a container image with `root` as the user was used, while `runAsNonRoot` is set. That combination is incompatible and Kubernetes fails to load the container in that case (as expected).

Now onto using the app. Create a proxy to the service.

```bash
$ kubectl port-forward service/dotnet-non-root 8080
Forwarding from 127.0.0.1:8080 -> 8080
Forwarding from [::1]:8080 -> 8080
```

View the sample app at `http://localhost:8080/` or call `curl http://localhost:8080/Environment`.

```bash
% curl http://localhost:8080/Environment
{"runtimeVersion":".NET 8.0.4","osVersion":"Ubuntu 22.04.4 LTS","osArchitecture":"Arm64","user":"app","processorCount":8,"totalAvailableMemoryBytes":4113563648,"memoryLimit":0,"memoryUsage":34082816,"hostName":"dotnet-non-root-8467576789-9dj8g"}
```

`user` is displayed as `app`, as expected.

Delete the resources (remote URL or local manifest).

```bash
kubectl delete -f https://raw.githubusercontent.com/dotnet/dotnet-docker/main/samples/kubernetes/non-root/non-root.yaml
```

## SecurityContext

[`SecurityContext`](https://kubernetes.io/docs/reference/generated/kubernetes-api/v1.24/#securitycontext-v1-core) holds security configuration that will be applied to a container, in a Pod or [Deployment manifest](non-root.yaml). These fields can be used to configure and/or validate that the container image is running as non-root. This sample follows [Kubernetes "Restricted" hardening best practices](https://kubernetes.io/docs/concepts/security/pod-security-standards/#restricted).

This [`securityContext`](https://kubernetes.io/docs/tasks/configure-pod-container/security-context/) object can be used to enforce non-root hosting:

```yml
    spec:
      securityContext:
        runAsNonRoot: true
      containers:
      - name: aspnetapp
        image: mcr.microsoft.com/dotnet/samples:aspnetapp-chiseled
        ports:
        - containerPort: 8080
        securityContext:
          allowPrivilegeEscalation: false
```

- `allowPrivilegeEscalation` -- Prevents (if `false`) a process from gaining greater privileges than its parent process. This is a good setting, but not directly related to users.
- `runAsNonRoot` -- Tests that the user (via uid) is a non-root user, otherwise fail.
- `runAsUser` -- Sets the user, via `UID`. Only needed if not set in the container image or overriding the container image user configuration. This setting is not present in the example above.

`securityContext` is set twice. In the first case, `runAsNonRoot` is set of all containers in the pod. There is only one in this example. Setting this property at the pod root sets clear intent and avoids duplication. `allowPrivilegeEscalation` can only be set for a specific container.

The `USER` Dockerfile instruction must be set via `UID` for the the `runAsNonRoot` setting to work correctly, as demonstrated by [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/7bca20cb06e1f912fc2e7fa8ce04dda606277537/samples/aspnetapp/Dockerfile#L21) and the following example.

```dockerfile
USER $APP_UID
```
