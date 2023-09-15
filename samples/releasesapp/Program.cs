using System.Text.Json;
using System.Text.Json.Serialization;
using ReportJson;

Report report = await ReleaseReport.Generator.MakeReportAsync();
string json = JsonSerializer.Serialize(report, AppJsonSerializerContext.Default.Report);
Console.WriteLine(json);

[JsonSourceGenerationOptions(
    GenerationMode = JsonSourceGenerationMode.Serialization,
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
    WriteIndented = true
    )]
[JsonSerializable(typeof(Report))]
internal partial class AppJsonSerializerContext : JsonSerializerContext
{
}
