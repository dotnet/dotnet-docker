using System.Text.Json;
using System.Text.Json.Serialization;

var report = await ReleaseReport.Generator.MakeReportAsync();
var json = JsonSerializer.Serialize(report, AppJsonSerializerContext.Default.Report);
Console.WriteLine(json);

[JsonSourceGenerationOptions(
    GenerationMode = JsonSourceGenerationMode.Serialization,
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase
    )]
[JsonSerializable(typeof(ReportJson.Report))]
internal partial class AppJsonSerializerContext : JsonSerializerContext
{
}