public class DefaultLogsService : LogsService.LogsServiceBase
{
    private readonly ILogger<DefaultLogsService> _logger;
    private readonly TelemetryResults _telemetryResults;

    public DefaultLogsService(ILogger<DefaultLogsService> logger, TelemetryResults telemetryResults)
    {
        _logger = logger;
        _telemetryResults = telemetryResults;
    }

    public override Task<ExportLogsServiceResponse> Export(ExportLogsServiceRequest request, ServerCallContext context)
    {
        foreach (var resource in request.ResourceLogs)
        {
            var resourceName = resource.Resource?.Attributes.FirstOrDefault(a => a.Key == OtlpApplication.SERVICE_NAME)?.Value.ValueString();
            _logger.LogDebug("Received {count} scope logs for resource {resourceName}", resource.ScopeLogs.Count, resourceName);

            foreach (var scope in resource.ScopeLogs)
            {
                _logger.LogDebug($"Received {scope.LogRecords.Count} log records for scope {scope.Scope?.Name}");
                _telemetryResults.LogMessageCount += scope.LogRecords.Count;
            }
        }

        var resp = new ExportLogsServiceResponse
        {
            PartialSuccess = null
        };

        return Task.FromResult(resp);
    }
}
