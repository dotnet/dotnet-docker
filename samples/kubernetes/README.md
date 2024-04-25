# Using .NET with Kubernetes

[Kubernetes](https://kubernetes.io/) is an orchestration system for containers. You can host .NET in Kubernetes in the same way as other dev platforms. We've provided samples that demonstrate best practices for configuring .NET apps with Kubernetes. More general container samples are provided in the [samples](../README.md) directory.

The samples were tested with:

- kubectl 1.30
- minikube 1.33 (Kubernetes 1.30.0)
- Docker Desktop 26.0.0 (Kubernetes 1.29.2)

The instruction rely on a correctly configured [Kubernetes environment](./environment.md).

## Run sample

Apply sample to your cluster.

```bash
kubectl apply -f https://raw.githubusercontent.com/dotnet/dotnet-docker/main/samples/kubernetes/hello-dotnet/hello-dotnet.yaml
kubectl port-forward service/hello-dotnet 8080
```

View the sample app at `http://localhost:8080/`. See [hello-dotnet](hello-dotnet/README.md) for information.

## Examples

The following examples demonstrate various Kubernetes and .NET capabilities.

- [hello-dotnet](hello-dotnet/README.md)
- [Manual deployment](manual-deployment/README.md)
- [Resource limits](resource-limits/README.md)
- [Non-root user](non-root/README.md)
- [Replicas and health checks](replicas-and-health/README.md)
- [dotnet-monitor](dotnet-monitor/README.md)
- [Graceful shutdown](graceful-shutdown/README.md)

The hello-dotnet example demonstrates multiple capabilities in a single sample.
