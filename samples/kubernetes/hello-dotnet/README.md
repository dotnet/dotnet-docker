# Configure a .NET app with Kubernetes

This .NET app sample is fully-configured (for a simple app) with respect to [Kubernetes](https://kubernetes.io/) settings. It also uses [`dotnet-monitor`](https://github.com/dotnet/dotnet-monitor) to gather diagnostic data via HTTP endpoints from the running application. It includes both `ClusterIP` and `LoadBalancer` variants, for local and cloud cluster use, respectively.

This sample uses a non-root user, has container limits set, uses dotnet- and is running on .NET 8.

`dotnet-monitor` relies on a volume mount to communicate with the app. It relies on running as the same user to share the mount.

## Run on your local cluster

Apply to your local cluster, using a `ClusterIP` service.

```bash
$ kubectl apply -f https://raw.githubusercontent.com/dotnet/dotnet-docker/main/samples/kubernetes/hello-dotnet/hello-dotnet.yaml
$ kubectl port-forward service/hello-dotnet 8080
```

View the sample app at http://localhost:8080/ and call `curl http://localhost:8080/Environment`.

See [Monitor your app with `dotnet-monitor` in Kubernetes](../dotnet-monitor/README.md) to learn how to collect `dotnet-monitor` metrics with [Prometheus](https://prometheus.io/).

## Run on your cloud cluster

Apply to your local cluster, using a `LoadBalancer` service.

```bash
$ kubectl apply -f https://raw.githubusercontent.com/dotnet/dotnet-docker/main/samples/kubernetes/hello-dotnet/hello-dotnet-loadbalancer.yaml
$ kubectl get service -w
```

You can discover the external IP for the service using `kubectl`.

```bash
% kubectl get service -w
NAME           TYPE           CLUSTER-IP     EXTERNAL-IP   PORT(S)        AGE
hello-dotnet   LoadBalancer   10.0.120.232   <pending>     80:31567/TCP   5s
kubernetes     ClusterIP      10.0.0.1       <none>        443/TCP        42h
hello-dotnet   LoadBalancer   10.0.120.232   20.51.80.224   80:31567/TCP   9s
```

Otherwise, you can configure your local environment to [create a `LoadBalancer` tunnel](https://minikube.sigs.k8s.io/docs/handbook/accessing/#example-of-loadbalancer), per whichever local cluster software you are using.

View the sample app at http://EXTERNAL-IP/ and call `curl http://EXTERNAL-IP/Environment`.

### Configure `kubectl` to access your cloud service

To run on Azure Kubernetes Service (AKS) or another cloud service, configure `kubectl` to access that kubernetes cluster control plane. For AKS, you can do that [via the Azure CLI](https://learn.microsoft.com/azure/aks/learn/quick-kubernetes-deploy-cli#connect-to-the-cluster). This same command is available via the "Connect" menu in the Azure Portal (for an AKS resource).

```bash
$ az aks get-credentials --resource-group myResourceGroup --name myAKSCluster
$ kubectl get nodes
NAME                                STATUS   ROLES   AGE   VERSION
aks-agentpool-81348477-vmss000004   Ready    agent   2d    v1.26.0
```

### Monitor with Prometheus

[Prometheus](https://prometheus.io/) is a popular monitoring solution. `dotnet-monitor` exports metrics data in [Prometheus exposition format](https://prometheus.io/docs/instrumenting/exposition_formats/) via its `metrics` endpoint. That makes it straightforward to connect the two systems.

You can use Prometheus in AKS to collect `dotnet-monitor` metrics. The endpoint `metrics` endpoint produdces Prometheus format metrics, so it makes sense.

```yaml
    metadata:
      annotations:
        prometheus.io/scrape: 'true'
        prometheus.io/path: '/metrics'
        prometheus.io/port: '52325'
        prometheus.io/scheme: 'http'
```

That metadata tells Prometheus where to look. Azure needs a bit more data to [Collect Prometheus metrics with Container insights](https://learn.microsoft.com/azure/azure-monitor/containers/container-insights-prometheus?tabs=pod). That's what [container-azm-ms-agentconfig.yaml](container-azm-ms-agentconfig.yaml) is for.

First, you need to enable [Azure Monitor managed service for Prometheus](https://learn.microsoft.com/azure/azure-monitor/essentials/prometheus-metrics-overview).

After that, you can need to apply `container-azm-ms-agentconfig.yaml` to kick off monitoring.

```bash
kubectl apply -f container-azm-ms-agentconfig.yaml
```

You should now drive some traffic to the site (via the browser or `curl`). After that, you should be able to run a query in **Logs** like the following, in Kusto.

```kusto
InsightsMetrics 
| where Namespace == "prometheus"
| summarize count() by Name
```

<img width="418" alt="image" src="https://user-images.githubusercontent.com/2608468/232164788-64f3fbfc-6310-4786-af64-59e22c21c5d7.png">

You can query specific metrics, like the following.

```kusto
InsightsMetrics 
| where Namespace == "prometheus"
| where Name == "microsoftaspnetcorehosting_requests_per_second"
| top 10 by Val
```
