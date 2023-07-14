using System.Text.Json;
using System.Text.Json.Serialization;
using ReleaseJson;
using Report;

var report = await ReportGenerator.MakeReport();
var json = JsonSerializer.Serialize(report, AppJsonSerializerContext.Default.ReleaseReport);
Console.WriteLine(json);

[JsonSourceGenerationOptions(
    GenerationMode = JsonSourceGenerationMode.Serialization,
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase
    )]
[JsonSerializable(typeof(ReleaseReport))]
internal partial class AppJsonSerializerContext : JsonSerializerContext
{
}