# Using a non-root user

The Kubernetes project provides [best practice guidance on Pod security](https://kubernetes.io/docs/concepts/security/pod-security-standards/#restricted). It requires using a non-root users in the containers you host.

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
kubectl port-forward service/hello-dotnet 8080:8080
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

[`SecurityContext`](https://kubernetes.io/docs/reference/generated/kubernetes-api/v1.24/#securitycontext-v1-core) offers a few options, as demonstrated by the following manifest fragment.

```yml
        securityContext:
          allowPrivilegeEscalation: false
          runAsNonRoot: true
        #   runAsUser: 64198
```

- [allowPrivilegeEscalation](https://kubernetes.io/docs/tasks/configure-pod-container/security-context/) -- Prevents (if `false`) a process from gaining great privileges  than its parent process.
- `runAsNonRoot` -- Tests that the container is run as a non-root user, otherwise fail.
- `runAsUser` -- Tests that container is run as a specific user (via uid), otherwise fail.

`runAsUser` is commented because it isn't strictly necessary. It is required in the following situations:

- The Dockerfile sets the `USER` via name not uid, since Kubernetes requires a uid.
- It is considered important to validate that the container is run as a specific non-root user.
