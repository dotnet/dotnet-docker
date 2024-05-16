# Monitor your app with `dotnet-monitor` in Kubernetes

[`dotnet-monitor`](https://github.com/dotnet/dotnet-monitor) allows you to gather diagnostic data from running applications using HTTP endpoints. It can be used as a sidecar for your app in your [Kubernetes](https://kubernetes.io/) deployment.

## Use `dotnet-monitor`

Apply [dotnet-monitor.yaml](dotnet-monitor.yaml) to your cluster.

```bash
kubectl apply -f https://raw.githubusercontent.com/dotnet/dotnet-docker/main/samples/kubernetes/dotnet-monitor/dotnet-monitor.yaml
```

Apply the local file if you've cloned the repo.

```bash
kubectl apply -f dotnet-monitor.yaml
```

Create a proxy to the service, on all three ports.

```bash
kubectl port-forward service/dotnet-monitor 8080 52323 52325
```

View the sample app at http://localhost:8080/ or call `curl http://localhost:8080/Environment`.

You can query the app.

```bash
% curl http://localhost:8080/Environment
{"runtimeVersion":".NET 8.0.5","osVersion":"Alpine Linux v3.19","osArchitecture":"X64","user":"root","processorCount":1,"totalAvailableMemoryBytes":78643200,"memoryLimit":104857600,"memoryUsage":34238464,"hostName":"dotnet-resource-limits-8685bd4b75-jltn5"}
```

You can query basic information from `dotnet-monitor` with the following approach.

```bash
% curl http://localhost:52323/info
{"version":"8.0.2-servicing.24258.10+25e00f40ba2a55512ea59365e73ddb4b27c73280","runtimeVersion":"8.0.5","diagnosticPortMode":"Listen","diagnosticPortName":"/diag/dotnet-monitor.sock"}
 % curl http://localhost:52323/processes
[{"pid":1,"uid":"6dd610b6-bc0c-4bf1-a882-4fa0afea34d2","name":"aspnetapp","isDefault":true}]   
% curl "http://localhost:52323/processes?pid=1"
[{"pid":1,"uid":"6dd610b6-bc0c-4bf1-a882-4fa0afea34d2","name":"aspnetapp","isDefault":true}]
```

See [API documentation](https://github.com/dotnet/dotnet-monitor/blob/main/documentation/api/README.md) for a complete list of endpoints.

The `livemetrics` endpoint provides access to more information.

```bash
% curl "http://localhost:52323/livemetrics?pid=1"
{"timestamp":"2023-04-11T02:05:22.4036612+00:00","provider":"Microsoft.AspNetCore.Hosting","name":"requests-per-second","displayName":"Request Rate","unit":"count","counterType":"Rate","tags":"","value":7}
{"timestamp":"2023-04-11T02:05:22.4037646+00:00","provider":"Microsoft.AspNetCore.Hosting","name":"total-requests","displayName":"Total Requests","unit":"","counterType":"Metric","tags":"","value":27}
{"timestamp":"2023-04-11T02:05:22.4037831+00:00","provider":"Microsoft.AspNetCore.Hosting","name":"current-requests","displayName":"Current Requests","unit":"","counterType":"Metric","tags":"","value":0}
{"timestamp":"2023-04-11T02:05:22.4037996+00:00","provider":"Microsoft.AspNetCore.Hosting","name":"failed-requests","displayName":"Failed Requests","unit":"","counterType":"Metric","tags":"","value":0}
```

The `metrics` endpoint exposes the same data in [Prometheus exposition format](https://prometheus.io/docs/instrumenting/exposition_formats/). Here, we're requested it on port `52325`.

```bash
% curl http://localhost:52325/metrics   
# HELP microsoftaspnetcorehosting_requests_per_second Request Rate
# TYPE microsoftaspnetcorehosting_requests_per_second gauge
microsoftaspnetcorehosting_requests_per_second 0 1681509071290
microsoftaspnetcorehosting_requests_per_second 0 1681509076289
microsoftaspnetcorehosting_requests_per_second 0 1681509081292
# HELP microsoftaspnetcorehosting_total_requests Total Requests
# TYPE microsoftaspnetcorehosting_total_requests gauge
microsoftaspnetcorehosting_total_requests 5 1681509066304
microsoftaspnetcorehosting_total_requests 5 1681509071290
microsoftaspnetcorehosting_total_requests 5 1681509076290
# HELP microsoftaspnetcorehosting_current_requests Current Requests
# TYPE microsoftaspnetcorehosting_current_requests gauge
microsoftaspnetcorehosting_current_requests 0 1681509066304
microsoftaspnetcorehosting_current_requests 0 1681509071290
microsoftaspnetcorehosting_current_requests 0 1681509076290
# HELP microsoftaspnetcorehosting_failed_requests Failed Requests
# TYPE microsoftaspnetcorehosting_failed_requests gauge
microsoftaspnetcorehosting_failed_requests 0 1681509066304
microsoftaspnetcorehosting_failed_requests 0 1681509071290
microsoftaspnetcorehosting_failed_requests 0 1681509076290
```

Port `52325` is special since it limits access to just the `metrics` endpoint, making it safe to expose to external systems. The other endpoints -- only accessible on port `52323` -- may provide access to privileged data, like dumps. This is why port `52323` is configured for the loopback interface, making it only accessible to the pod.

## Monitor with Prometheus

[Prometheus](https://prometheus.io/) is a popular monitoring solution. `dotnet-monitor` exports metrics data in [Prometheus exposition format](https://prometheus.io/docs/instrumenting/exposition_formats/) via its `metrics` endpoint. That makes it straightforward to connect the two systems.

Apply [prometheus-app.yaml](prometheus-app.yaml) to your cluster.

```bash
kubectl apply -f https://raw.githubusercontent.com/dotnet/dotnet-docker/main/samples/kubernetes/dotnet-monitor/prometheus-app.yaml
```

Apply the local file if you've cloned the repo.

```bash
kubectl apply -f prometheus-app.yaml
```

Create a proxy to the `prometheus` service.

```bash
kubectl port-forward service/prometheus 9090
```

View the Prometheus site at `http://localhost:9090`.

It should looks something like the following image.

<img width="1191" alt="image" src="https://user-images.githubusercontent.com/2608468/231349237-69bd3b08-57fd-4d87-9e16-1fdaf6087b34.png">

The "metrics explorer" icon to the left of the "Execute" button provides a list of metrics that have been collected.

## Delete resources

Delete the resources (remote URL or local manifest).

```bash
kubectl delete -f https://raw.githubusercontent.com/dotnet/dotnet-docker/main/samples/kubernetes/dotnet-monitor/dotnet-monitor.yaml
kubectl delete -f https://raw.githubusercontent.com/dotnet/dotnet-docker/main/samples/kubernetes/dotnet-monitor/prometheus-app.yaml
```
