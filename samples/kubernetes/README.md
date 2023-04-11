# Using .NET with Kubernetes

[Kubernetes](https://kubernetes.io/) provides an orchestration system for containers. You can host .NET in Kubernetes in the same way as other dev platforms. We've provided samples that demonstrate best practices for configuring .NET apps with Kubernetes. More general container samples are provided in the [samples](../README.md) directory.

These instruction use [kubectl](https://kubernetes.io/docs/reference/kubectl/) and should work with any [Kubernetes](https://kubernetes.io/) environment.

If you are new to Kubernetes, you'll need to establish a [local environment](https://kubernetes.io/docs/tasks/tools/), such as [Docker Desktop](https://www.docker.com/products/kubernetes/), [K3s](https://k3s.io/), [OpenShift Local](https://developers.redhat.com/products/openshift-local), and [Rancher](https://rancherdesktop.io/).

## Examples

The following fine-grained examples demonstrate various Kubernetes and .NET capabilities.

- [Resource limits](resource-limits/README.md)
- [Replicas and health checks](health-and-replicas/README.md)
- [Non-root user](non-root-user/README.md)
- [Graceful shutdown](graceful-shutdown/README.md)
- [Manual deployment](manual-deployment/README.md)

The [`hello-dotnet`](hello-dotnet/README.md) example is demonstrates most of these aspects together, in a real-world configuration.  `NodePort` and `LoadBalancer` variants are provided.

## Run .NET app on your local cluster

You can host a `NodePort` sample on your local cluster:

```bash
$ kubectl apply -f https://raw.githubusercontent.com/dotnet/dotnet-docker/main/samples/kubernetes/hello-dotnet/hello-dotnet-nodeport.yaml
$ kubectl port-forward service/hello-dotnet 8080:80
```

## Run .NET app on your cloud cluster

You can host a `LoadBalancer` sample on your cloud cluster:

```bash
$ kubectl apply -f https://raw.githubusercontent.com/dotnet/dotnet-docker/main/samples/kubernetes/hello-dotnet/hello-dotnet-loadbalancer.yaml
$ kubectl get service -w
```
