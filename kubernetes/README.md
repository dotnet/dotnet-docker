# Using .NET with Kubernetes

[Kubernetes](https://kubernetes.io/) provides an orchestration system for containers. You can host .NET in Kubernetes in the same way as other dev platforms. We've provided samples that demonstrate best practices for configuring .NET apps with Kubernetes. More general container samples are provided in the [samples](../samples/README.md) directory.

These instruction should work with any [Kubernetes](https://kubernetes.io/) environment. The instructions make extensive use of [kubectl](https://kubernetes.io/docs/reference/kubectl/).

You can host a .NET sample with a [few quick commands](hello-dotnet/README.md).

```bash
kubectl apply -f https://raw.githubusercontent.com/dotnet/dotnet-docker/main/kubernets/hello-dotnet/hello-dotnet.yaml
kubectl port-forward service/hello-dotnet 8080:80
```

If you are new to Kubernetes, you'll need to establish a [local environment](https://kubernetes.io/docs/tasks/tools/), such as [Docker Desktop](https://www.docker.com/products/kubernetes/), [K3s](https://k3s.io/) and [Rancher](https://rancherdesktop.io/).

A subset of patterns are covered to help you get started.

- [Single node app](hello-dotnet/README.md)
- [Resource limits](resource-limits/README.md)
- [Replicas and health checks](health-and-replicas/README.md)
- [Graceful shutdown](graceful-shutdown/README.md)
