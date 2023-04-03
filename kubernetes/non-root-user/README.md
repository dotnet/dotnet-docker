# Using a non-root user

Use a non-root user with a [sample app](../../samples/aspnetapp/Dockerfile.alpine-non-root).

Note: This sample uses a .NET 8 container image, which includes a non-root user.

Launch an app on your cluster with the following command.

```bash
kubectl apply -f https://raw.githubusercontent.com/dotnet/dotnet-docker/main/kubernetes/non-root-user/non-root-user.yaml
```

Or use the manifest directly if you've cloned the repo.

```bash
kubectl apply -f non-root-user.yaml
```

Create a proxy to the service.

```bash
kubectl port-forward service/dotnet-non-root 8080:8080
```

View the sample app at http://localhost:8080/ or call `curl http://localhost:8080/Environment`.

View the active resources that have been deployed and then delete them.

```bash
kubectl get pod
kubectl get service
kubectl get deployment
```

Delete the resources (remote URL or local manifest).

```bash
kubectl delete -f https://raw.githubusercontent.com/dotnet/dotnet-docker/main/kubernetes/non-root-user/non-root-user.yaml
```

## SecurityContext

[`SecurityContext`](https://kubernetes.io/docs/reference/generated/kubernetes-api/v1.24/#securitycontext-v1-core) holds security configuration that will be applied to a container, in a Pod or [Deployment manifest](non-root-user.yaml). It includes a few fields that are required to configure a container to correctly run as a non-root user. This sample follows [Kubernetes "Restricted" hardening best practices](https://kubernetes.io/docs/concepts/security/pod-security-standards/#restricted).

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
