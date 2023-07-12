using System.Text.Json.Serialization;

var builder = WebApplication.CreateSlimBuilder(args);
builder.Services.AddHealthChecks();

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
});

var app = builder.Build();

app.MapHealthChecks("/healthz");

CancellationTokenSource cancellation = new();
app.Lifetime.ApplicationStopping.Register( () =>
{
    cancellation.Cancel();
});

app.MapGet("/Environment", () =>
{
    return new EnvironmentInfo();
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
    
    return new Operation(value);
});

app.Run();

[JsonSerializable(typeof(EnvironmentInfo))]
[JsonSerializable(typeof(Operation))]
internal partial class AppJsonSerializerContext : JsonSerializerContext
{
}

public record Operation(int Delay);
