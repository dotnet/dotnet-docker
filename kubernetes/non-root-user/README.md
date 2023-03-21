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

- [allowPrivilegeEscalation](https://kubernetes.io/docs/tasks/configure-pod-container/security-context/) -- Prevents (if `false`) a process from gaining great privileges than its parent process. This is a good setting, but not directly related to users.
- `runAsNonRoot` -- Tests that the user (via uid) is a non-root user, otherwise fail.

`runAsUser` isn't present in this example, but is a related option. `runAsNonRoot` is a test that a non-root user is used, per the `USER` instruction in container image metadata. `runAsUser` sets the user to a given UID. If `runAsNonRoot` is set to `true` and the `USER` in the container image is set by name and not UID, then `runAsUser` is required. In our example, `USER` is set by UID.

## Best practice

Users should be set by UID. .NET container images expose an environment variable for the UID, making that easy.

```dockerfile
USER $APP_UID
```

The source [Dockerfile](../../samples/aspnetapp/Dockerfile.alpine-non-root) for the `dotnetnonroot.azurecr.io/aspnetapp` image [demonstrates this pattern](https://github.com/dotnet/dotnet-docker/blob/main/samples/aspnetapp/Dockerfile.alpine-non-root#L28).

This approach has the advantage of making Dockerfiles easy to inspect and secure by default, while establishing Kubernetes manifests as the final arbiter of deployment policies.
