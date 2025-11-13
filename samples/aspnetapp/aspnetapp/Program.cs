using System.Text.Json.Serialization;
using aspnetapp;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddHealthChecks();

// Enable source generated JSON serialization
// https://learn.microsoft.com/dotnet/standard/serialization/system-text-json/source-generation#source-generation-support-in-aspnet-core
builder.Services.AddControllers().AddJsonOptions(options =>
    options.JsonSerializerOptions.TypeInfoResolverChain.Add(SampleAppJsonSerializerContext.Default));

var app = builder.Build();

app.MapHealthChecks("/healthz");

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.MapGet("/Environment", () => new EnvironmentInfo());

// This API demonstrates how to use task cancellation
// to support graceful container shutdown via SIGTERM.
// The method itself is an example and not useful.
var cancellation = new CancellationTokenSource();
app.Lifetime.ApplicationStopping.Register(cancellation.Cancel);
app.MapGet("/Delay/{value}", async (int value) =>
{
    try
    {
        await Task.Delay(value, cancellation.Token);
    }
    catch (TaskCanceledException)
    {
    }

    return new Operation(value);
});

app.Run();

// Enable source generated JSON serialization
// https://learn.microsoft.com/dotnet/standard/serialization/system-text-json/source-generation#source-generation-support-in-aspnet-core
[JsonSerializable(typeof(EnvironmentInfo))]
[JsonSerializable(typeof(Operation))]
internal partial class SampleAppJsonSerializerContext : JsonSerializerContext { }
