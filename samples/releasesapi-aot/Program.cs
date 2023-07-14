using System.Text.Json.Serialization;
using ReleaseJson;
using Report;

var builder = WebApplication.CreateSlimBuilder(args);
builder.Services.AddHealthChecks();

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
});

var app = builder.Build();

app.MapHealthChecks("/healthz");

app.MapGet("/releases", async () =>
{
    return await ReportGenerator.MakeReport();
});


app.Run();


[JsonSerializable(typeof(ReleaseReport))]
internal partial class AppJsonSerializerContext : JsonSerializerContext
{
}
