# Using .NET with Kubernetes

[Kubernetes](https://kubernetes.io/) provides an orchestration system for containers. You can host .NET in Kubernetes in the same way as other dev platforms. We've provided samples that demonstrate best practices for configuring .NET apps with Kubernetes. More general container samples are provided in the [samples](../README.md) directory.

These instruction should work with any [Kubernetes](https://kubernetes.io/) environment. The instructions make extensive use of [kubectl](https://kubernetes.io/docs/reference/kubectl/).

If you are new to Kubernetes, you'll need to establish a [local environment](https://kubernetes.io/docs/tasks/tools/), such as [Docker Desktop](https://www.docker.com/products/kubernetes/), [K3s](https://k3s.io/) and [Rancher](https://rancherdesktop.io/).

## Examples

Two examples are provided, one simple and the other more real-world. Both of these examples provide both `NodePort` and `LoadBalancer` variants.

- [Basic configuration](basic-dotnet/README.md)
- [Real-world configuration](hello-dotnet/README.md)

A subset of patterns are also provided.

- [Resource limits](resource-limits/README.md)
- [Replicas and health checks](health-and-replicas/README.md)
- [Graceful shutdown](graceful-shutdown/README.md)

## Run .NET app on your local cluster

You can host a .NET sample on your local cluster with a few quick commands [kubectl](https://kubernetes.io/docs/reference/kubectl/) commands.

```bash
kubectl apply -f https://raw.githubusercontent.com/dotnet/dotnet-docker/main/samples/kubernetes/hello-dotnet/hello-dotnet.yaml
kubectl port-forward service/hello-dotnet 8080:80
```

