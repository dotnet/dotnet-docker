# Using .NET with Kubernetes

[Kubernetes](https://kubernetes.io/) is an orchestration system for containers. You can host .NET in Kubernetes in the same way as other dev platforms. We've provided samples that demonstrate best practices for configuring .NET apps with Kubernetes. More general container samples are provided in the [samples](../README.md) directory.

These instruction use [kubectl](https://kubernetes.io/docs/reference/kubectl/) and should work with any [Kubernetes](https://kubernetes.io/) environment.

If you are new to Kubernetes, you'll need to establish a [local environment](https://kubernetes.io/docs/tasks/tools/), such as [Docker Desktop](https://www.docker.com/products/kubernetes/), [K3s](https://k3s.io/), [OpenShift Local](https://developers.redhat.com/products/openshift-local), and [Rancher](https://rancherdesktop.io/).

## Examples

The following fine-grained examples demonstrate various Kubernetes and .NET capabilities.

- [Manual deployment](manual-deployment/README.md)
- [Resource limits](resource-limits/README.md)
- [Non-root user](non-root-user/README.md)
- [Replicas and health checks](health-and-replicas/README.md)
- [Graceful shutdown](graceful-shutdown/README.md)
- [Fully configured](hello-dotnet/README.md)

The [Fully configured](hello-dotnet/README.md) example demonstrates most of these aspects together.  [`ClusterIP` and `LoadBalancer`](https://minikube.sigs.k8s.io/docs/handbook/accessing/) [service variants](https://learn.microsoft.com/azure/aks/concepts-network#services) are provided, enabling straightforward local and cloud testing, respectively.

## Run app on your local cluster

Apply to your local cluster, using a `ClusterIP` service.

```bash
$ kubectl apply -f https://raw.githubusercontent.com/dotnet/dotnet-docker/main/samples/kubernetes/hello-dotnet/hello-dotnet.yaml
$ kubectl port-forward service/hello-dotnet 8080:80
```

## Run app on your cloud cluster

Apply to your cloud cluster, using a `LoadBalancer` service.

```bash
$ kubectl apply -f https://raw.githubusercontent.com/dotnet/dotnet-docker/main/samples/kubernetes/hello-dotnet/hello-dotnet-loadbalancer.yaml
$ kubectl get service -w
```
