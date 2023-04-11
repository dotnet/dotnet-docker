# Monitor your app with `dotnet-monitor` in Kubernetes

[`dotnet-monitor`](https://github.com/dotnet/dotnet-monitor) allows you to gather diagnostic data from running applications using HTTP endpoints. It can be used as a sidecar for your app in your [Kubernetes](https://kubernetes.io/) deployment.

Apply [dotnet-monitor.yaml](dotnet-monitor.yaml) to your cluster.

```bash
kubectl apply -f https://raw.githubusercontent.com/dotnet/dotnet-docker/main/samples/kubernetes/dotnet-monitor/dotnet-monitor.yaml
```

Apply the local file if you've cloned the repo.

```bash
kubectl apply -f dotnet-monitor.yaml
```

Create a proxy to the `aspnetapp` service.

```bash
kubectl port-forward service/aspnetapp 8080:80
```

View the sample app at http://localhost:8080/ or call `curl http://localhost:8080/Environment`.

Create a proxy to the `monitor` service.

```bash
kubectl port-forward service/monitor 52323
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
% kubectl delete -f load-test.yaml
```

Note: The job must be deleted and then re-applied to be run again.

Resources can be deleted using the following pattern:

```bash
kubectrl delete -f https://raw.githubusercontent.com/dotnet/dotnet-docker/main/samples/kubernetes/resource-limits/resource-limits.yaml
```
