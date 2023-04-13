# Graceful shutdown of ASP.NET-based applications

Kubernetes-hosted applications should shut down smoothly and quickly when asked to do so. When they do, time necessary to upgrade deployments, to scale deployments down, and to migrate pods from overloaded/misbehaving nodes are all reduced. Even more importantly, being aware of the fact that Kubernetes requested a pod to shut down, gives the pod an opportunity to gracefully end all work in progress, improving system reliability.

## How Kubernetes requests application shutdown

Kubernetes will send `SIGTERM` signal to the main container process as means of requesting shutdown. By default the container has 30 seconds to exit gracefully before it gets killed, but this can be changed via [container lifecycle spec](https://kubernetes.io/docs/reference/kubernetes-api/workload-resources/pod-v1/#lifecycle).

The default ASP.NET host [handles SIGTERM signal](https://learn.microsoft.com/aspnet/core/fundamentals/host/generic-host#ihostlifetime) and will raise the `ApplicationStopping` event when that happens. The host will stop accepting new requests, but the requests already "in flight" will run to completion, subject to a timeout. The default shutdown timeout in ASP.NET is 30 seconds, and can be changed via `HostOptions.ShutdownTimeout`, for example:

```csharp
builder.Host.ConfigureHostOptions(opts => opts.ShutdownTimeout = TimeSpan.FromSeconds(20));
```

Changing the `HostOptions.ShutdownTimeout` is the simplest way to modify the application shutdown, but relying on this setting exclusively means requests in flight will be aborted when the timeout is reached. Clients that sent these requests will be notified (ASP.NET and the operating system will close client connections automatically) but request handlers will not run to completion. If you need to cancel request work gracefully, you can use `ApplicationShutdown` event to get notified when a shutdown is requested. How you handle the event, varies depending on what type of code is performing a long-running operation. There are two broad categories of such code: long-running requests and background processing.

## Long-running network requests

In general, it is better to avoid situations where a network request takes a long time to process (several seconds or more). These kinds of requests tend to limit application scalability and are prone to failing due to network connection errors and client timeouts. It is better, if possible, to replace a long-running requests with a pair of related requests: the first request starts the operation, and the second request allows the client to ask for the result. If this is not feasible, you can make long-running requests cancel work promptly (and avoid being aborted by the ASP.NET host) by leveraging the `CancellationToken` that is associated with `ApplicationStopping` event.

Any ASP.NET request handler [can take a `CancellationToken` as a parameter](https://learn.microsoft.com/aspnet/core/fundamentals/minimal-apis/parameter-binding?view=aspnetcore-7.0#special-types). This `CancellationToken` is automatically created by ASP.NET and will be activated when the request is cancelled by the client. By default, the same `CancellationToken` will NOT be activated when `ApplicationStopping` event is raised, but you can create a "linked" `CancellationToken` to cover both cases. Here is how to do it with ASP.NET minimal API framework:

```csharp
app.Map("/longop/{value}", async Task<Results<StatusCodeHttpResult, Ok<String>>> (int value, CancellationToken requestCt, [FromServices] IHostApplicationLifetime hostLifetime) =>
{
    var effectiveCt = CancellationTokenSource.CreateLinkedTokenSource(requestCt, hostLifetime.ApplicationStopping).Token;

    try
    {
        // Simulates long processing...
        await Task.Delay(value * 1000, effectiveCt);
    } 
    catch (OperationCanceledException)
    {
        // Will happen if client cancels the request OR when the app is shutting down.
        return TypedResults.StatusCode(StatusCodes.Status503ServiceUnavailable);
    }
    
    return TypedResults.Ok($"Worked {value} seconds, looks good");
});
```

The same technique can be applied to traditional (controller-based) ASP.NET code. You just need to add `IHostApplicationLifetime` parameter to controller method(s) that might take a long time to execute, construct a linked `CancellationToken`, and then use it inside the request method.

You can also change ASP.NET behavior so that the default `CancellationToken` passed to your request handlers *does get activated* when `ApplicationStopping` event occurs. This code snippet (a custom middleware) will do it:

```csharp
app.Use((httpContext, next) =>
{
    var hostLifetime = httpContext.RequestServices.GetRequiredService<IHostApplicationLifetime>();
    var originalCt = httpContext.RequestAborted;
    var combinedCt = CancellationTokenSource.CreateLinkedTokenSource(originalCt, hostLifetime.ApplicationStopping).Token;
    httpContext.RequestAborted = combinedCt;
    return next(httpContext);
});
```

## Adding a shutdown delay

Services that handle requests coming from [Kubernetes ingress](https://kubernetes.io/docs/concepts/services-networking/ingress/) may run into an issue where they continue to receive new requests *after* Kubernetes requests them to shut down via `SIGTERM` signal. This usually lasts for only a few seconds, but if the service shuts down immediately and these requests fail, it puts a burden on the client to retry them and the overall service responsiveness goes down.

The reason for this behavior is that ingress and Kubernetes control plane are separate entities, operating independently. When Kubernetes decides to shut down some pod(s), there is inevitable delay between control plane taking a pod out of Service backend, and ingress "noticing" that this has happened and actually stopping routing requests to that pod. So it is useful to introduce some delay between the time the pod receivers `SIGTERM` signal, and the time when it stops responding to requests. You can make it happen by replacing the default `IHostLifetime` implementation (which shuts down the application immediately upon receiving `SIGTERM` signal):

```csharp
using System.Runtime.InteropServices;

public class DelayedShutdownHostLifetime : IHostLifetime, IDisposable
{
    private IHostApplicationLifetime _applicationLifetime;
    private TimeSpan _delay;
    private IEnumerable<IDisposable>? _disposables;

    public DelayedShutdownHostLifetime(IHostApplicationLifetime applicationLifetime, TimeSpan delay) { 
        _applicationLifetime = applicationLifetime;
        _delay = delay;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task WaitForStartAsync(CancellationToken cancellationToken)
    {
        _disposables = new IDisposable[]
        {
            PosixSignalRegistration.Create(PosixSignal.SIGINT, HandleSignal),
            PosixSignalRegistration.Create(PosixSignal.SIGQUIT, HandleSignal),
            PosixSignalRegistration.Create(PosixSignal.SIGTERM, HandleSignal)
        };
        return Task.CompletedTask;
    }

    protected void HandleSignal(PosixSignalContext ctx)
    {
        ctx.Cancel = true;
        Task.Delay(_delay).ContinueWith(t => _applicationLifetime.StopApplication());
    }

    public void Dispose()
    {
        foreach (var disposable in _disposables ?? Enumerable.Empty<IDisposable>()) 
        {
            disposable.Dispose(); 
        }
    }
}
```

> Note: do not confuse `IHostLifetime` with `IHostApplicationLifetime`--they are related, but different interfaces. It is the `IHostLifetime` instance that listens for signals, including `SIGTERM`, and once a signal arrives, it calls into `IHostApplicationLifetime` instance to orchestrate application shutdown. For more information see [generic host shutdown documentation](https://learn.microsoft.com/dotnet/core/extensions/generic-host#host-shutdown)

Make ASP.NET use `DelayedShutdownHostLifetime` by adding it to the dependency injection container:

```csharp
builder.Services.AddSingleton<IHostLifetime>(sp => new DelayedShutdownHostLifetime(
    sp.GetRequiredService<IHostApplicationLifetime>(), 
    TimeSpan.FromSeconds(5) // ... or whatever delay is appropriate for your service.
));
```

The `IHostApplicationLifetime.ApplicationStopping` token will now be activated only after the specified delay after `SIGTERM` arrival.

## Background services

For background services ASP.NET provides [`IHostedService` interface and a `BackgroundService` base class](https://learn.microsoft.com/aspnet/core/fundamentals/host/hosted-services?view=aspnetcore-7.0) that allow for easy handling of `ApplicationStopping` event:

- If you are using `BackgroundService` base class, the `CancellationToken` passed to [`ExecuteAsync` method](https://learn.microsoft.com/aspnet/core/fundamentals/host/hosted-services?view=aspnetcore-7.0#backgroundservice-base-class) will be activated if `ApplicationStopping` event occurs. So as long as you are using this token, or a token derived from it, for all asynchronous calls, they will be automatically cancelled upon application shutdown.

- If you are implementing `IHostedService` interface directly, make sure you cancel all background processing when the framework calls [`StopAsync` method](https://learn.microsoft.com/aspnet/core/fundamentals/host/hosted-services?view=aspnetcore-7.0#stopasync) on your service. This usually means you need to keep a reference, or a `CancellationTokenSource`, for all background tasks in progress, so that you can cancel them when `StopAsync` is called.
