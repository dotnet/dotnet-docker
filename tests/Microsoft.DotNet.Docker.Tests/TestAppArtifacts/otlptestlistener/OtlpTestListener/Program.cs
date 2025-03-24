using OtlpTestListener.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<TelemetryResults>();

// Add services to the container.
builder.Services.AddGrpc();

var otlpPort = builder.Configuration.GetValue("OTLP_PORT", 4317);
var webPorts = builder.Configuration.GetValue("ASPNETCORE_HTTP_PORTS", "8080")!.Split(',').Select(p => int.Parse(p)).ToArray();

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(otlpPort, listenOptions =>
    {
        listenOptions.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http2;
    });
    foreach (var port in webPorts)
    {
        options.ListenAnyIP(port, listenOptions =>
        {
            listenOptions.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http1AndHttp2;
        });
    }
});


var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<DefaultMetricsService>();
app.MapGrpcService<DefaultTraceService>();
app.MapGrpcService<DefaultLogsService>();


app.MapGet("/report", (TelemetryResults tr) => tr.GetResultsJSON());
app.MapGet("/clear", (TelemetryResults tr) => tr.Clear());

app.Run();
