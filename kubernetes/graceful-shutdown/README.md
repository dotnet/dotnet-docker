# Graceful shutdown

Enable graceful shutdown of an app, particularly with long-running code.

The [following pattern](https://github.com/richlander/dotnet-docker/blob/09d257d8b75773569cd59f7cde283625d8fc7d42/samples/aspnetapp/aspnetapp/Program.cs#L30-L54) can be used:

```bash
CancellationTokenSource cancellation = new();
app.Lifetime.ApplicationStopping.Register( () =>
{
    cancellation.Cancel();
});

// This API demonstrates how to use task cancellation
// to support graceful container shutdown via SIGTERM.
// The method itself is an example and not useful.
app.MapGet("/Delay/{value}", async (int value) =>
{
    try
    {
        await Task.Delay(value, cancellation.Token);
    }
    catch(TaskCanceledException)
    {
    }
    
    return new DelayValue(value);
});
```

Test the pattern with a single replica deployment. This test requires multiple terminal windows (with careful timing). The pod names and other data won't look the same.

First terminal:

```bash
$ kubectl apply -f https://raw.githubusercontent.com/richlander/dotnet-k8s/main/hello-dotnet/hello-dotnet.yaml
$ kubectl port-forward service/hello-dotnet 8080:80
```

Second terminal:

```bash
$ kubectl get -w po
NAME                            READY   STATUS    RESTARTS   AGE
hello-dotnet-6fd9fc7cd8-msj9g   0/1     Pending   0          0s
hello-dotnet-6fd9fc7cd8-msj9g   0/1     Pending   0          0s
hello-dotnet-6fd9fc7cd8-msj9g   0/1     ContainerCreating   0          0s
hello-dotnet-6fd9fc7cd8-msj9g   1/1     Running             0          3s
hello-dotnet-6fd9fc7cd8-msj9g   1/1     Terminating         0          74s
hello-dotnet-6fd9fc7cd8-znmfj   0/1     Pending             0          0s
hello-dotnet-6fd9fc7cd8-znmfj   0/1     Pending             0          0s
hello-dotnet-6fd9fc7cd8-znmfj   0/1     ContainerCreating   0          0s
```

Third terminal:

```bash
$ kubectl get po
NAME                            READY   STATUS    RESTARTS   AGE
hello-dotnet-6fd9fc7cd8-msj9g   1/1     Running   0          88s
$ kubectl logs -f hello-dotnet-6fd9fc7cd8-msj9g
warn: Microsoft.AspNetCore.DataProtection.Repositories.FileSystemXmlRepository[60]
      Storing keys in a directory '/root/.aspnet/DataProtection-Keys' that may not be persisted outside of the container. Protected data will be unavailable when container is destroyed.
warn: Microsoft.AspNetCore.DataProtection.KeyManagement.XmlKeyManager[35]
      No XML encryptor configured. Key {06fa0193-972e-43a7-9da9-01c23ee0b99e} may be persisted to storage in unencrypted form.
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://[::]:80
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
info: Microsoft.Hosting.Lifetime[0]
      Hosting environment: Production
info: Microsoft.Hosting.Lifetime[0]
      Content root path: /app
warn: Microsoft.AspNetCore.HttpsPolicy.HttpsRedirectionMiddleware[3]
      Failed to determine the https port for redirect.
```

Fourth terminal:

```bash
$ curl http://localhost:8080/Delay/500
{"delay":500}
$ curl http://localhost:8080/Delay/50000
```

You have 50s to perform the task in the fifth terminal

Fifth terminal:

```bash
$ kubectl delete pod  hello-dotnet-6fd9fc7cd8-msj9g
pod "hello-dotnet-6fd9fc7cd8-msj9g" deleted
```

This command should take ~1s.

Let's look at the other terminal windows to see what happened.

Second terminal:

```bash
$ kubectl get -w po
NAME                            READY   STATUS    RESTARTS   AGE
hello-dotnet-6fd9fc7cd8-msj9g   0/1     Pending   0          0s
hello-dotnet-6fd9fc7cd8-msj9g   0/1     Pending   0          0s
hello-dotnet-6fd9fc7cd8-msj9g   0/1     ContainerCreating   0          0s
hello-dotnet-6fd9fc7cd8-msj9g   1/1     Running             0          3s
hello-dotnet-6fd9fc7cd8-msj9g   1/1     Terminating         0          74s
hello-dotnet-6fd9fc7cd8-znmfj   0/1     Pending             0          0s
hello-dotnet-6fd9fc7cd8-znmfj   0/1     Pending             0          0s
hello-dotnet-6fd9fc7cd8-znmfj   0/1     ContainerCreating   0          0s
hello-dotnet-6fd9fc7cd8-msj9g   0/1     Terminating         0          76s
hello-dotnet-6fd9fc7cd8-msj9g   0/1     Terminating         0          76s
hello-dotnet-6fd9fc7cd8-msj9g   0/1     Terminating         0          76s
hello-dotnet-6fd9fc7cd8-znmfj   1/1     Running             0          3s
```

The pod quickly recycled upon deletion.

Third terminal:

```bash
$ kubectl logs -f hello-dotnet-6fd9fc7cd8-msj9g
warn: Microsoft.AspNetCore.DataProtection.Repositories.FileSystemXmlRepository[60]
      Storing keys in a directory '/root/.aspnet/DataProtection-Keys' that may not be persisted outside of the container. Protected data will be unavailable when container is destroyed.
warn: Microsoft.AspNetCore.DataProtection.KeyManagement.XmlKeyManager[35]
      No XML encryptor configured. Key {06fa0193-972e-43a7-9da9-01c23ee0b99e} may be persisted to storage in unencrypted form.
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://[::]:80
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
info: Microsoft.Hosting.Lifetime[0]
      Hosting environment: Production
info: Microsoft.Hosting.Lifetime[0]
      Content root path: /app
warn: Microsoft.AspNetCore.HttpsPolicy.HttpsRedirectionMiddleware[3]
      Failed to determine the https port for redirect.
info: Microsoft.Hosting.Lifetime[0]
      Application is shutting down...
```

ASP.NET Core gracefully shutdown.
