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
{"runtimeVersion":".NET 7.0.5","osVersion":"Linux 5.15.49-linuxkit #1 SMP PREEMPT Tue Sep 13 07:51:32 UTC 2022","osArchitecture":"Arm64","user":"root","processorCount":1,"totalAvailableMemoryBytes":78643200,"memoryLimit":104857600,"memoryUsage":28311552}
```

You can query basic information from `dotnet-monitor` with the following approach.

```bash
% curl http://localhost:52323/info           
{"version":"7.1.0-rtm.23153.4+97a9c61787944de12efc766ca6ac825b3b9424f3","runtimeVersion":"7.0.4","diagnosticPortMode":"Listen","diagnosticPortName":"/diag/dotnet-monitor.sock"}
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

## Generate load

You may want to generate more traffic to demonstrate that `dotnet-monitor` can work well in production.

Apply the [`load-test.yaml`](load-test.yaml) manifest. Start collecting `livemetrics` just before doing (in another terminal).

```bash
% curl "http://localhost:52323/livemetrics?pid=1"
{"timestamp":"2023-04-11T02:16:37.4180174+00:00","provider":"Microsoft.AspNetCore.Hosting","name":"requests-per-second","displayName":"Request Rate","unit":"count","counterType":"Rate","tags":"","value":31937}
{"timestamp":"2023-04-11T02:16:37.4180254+00:00","provider":"Microsoft.AspNetCore.Hosting","name":"total-requests","displayName":"Total Requests","unit":"","counterType":"Metric","tags":"","value":409508}
{"timestamp":"2023-04-11T02:16:37.418029+00:00","provider":"Microsoft.AspNetCore.Hosting","name":"current-requests","displayName":"Current Requests","unit":"","counterType":"Metric","tags":"","value":9}
{"timestamp":"2023-04-11T02:16:37.4180323+00:00","provider":"Microsoft.AspNetCore.Hosting","name":"failed-requests","displayName":"Failed Requests","unit":"","counterType":"Metric","tags":"","value":0}
```

Delete the job.

```bash
kubectl delete -f load-test.yaml
```

Note: The job must be deleted and then re-applied to be run again.

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

You can re-run the load-test job on the `aspnetapp` site to generate data for Prometheus to collect via `dotnet-monitor`. You can run it multiple times to generate more traffic. It runs for a minute. The job needs to be deleted before it can be re-applied.

It should looks something like the following image.

<img width="1191" alt="image" src="https://user-images.githubusercontent.com/2608468/231349237-69bd3b08-57fd-4d87-9e16-1fdaf6087b34.png">

The "metrics explorer" icon to the left of the "Execute" button provides a list of metrics that have been collected.

## Delete resources

Delete the resources (remote URL or local manifest).

```bash
kubectrl delete -f https://raw.githubusercontent.com/dotnet/dotnet-docker/main/samples/kubernetes/dotnet-monitor/dotnet-monitor.yaml
kubectrl delete -f https://raw.githubusercontent.com/dotnet/dotnet-docker/main/samples/kubernetes/dotnet-monitor/load-test.yaml
kubectrl delete -f https://raw.githubusercontent.com/dotnet/dotnet-docker/main/samples/kubernetes/dotnet-monitor/prometheus-app.yaml
```
