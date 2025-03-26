namespace OtlpTestListener.Services;

public class DefaultMetricsService : MetricsService.MetricsServiceBase
{
    private readonly ILogger<DefaultMetricsService> _logger;
    private readonly TelemetryResults _telemetryResults;
    public DefaultMetricsService(ILogger<DefaultMetricsService> logger, TelemetryResults telemetryResults)
    {
        _logger = logger;
        _telemetryResults = telemetryResults;
    }

    public override Task<ExportMetricsServiceResponse> Export(ExportMetricsServiceRequest request, ServerCallContext context)
    {
        foreach (var resource in request.ResourceMetrics)
        {
            var resourceName = resource.Resource?.Attributes.FirstOrDefault(a => a.Key == OtlpApplication.SERVICE_NAME)?.Value.ValueString();
            _logger.LogDebug("Received {count} scope metrics for resource {resourceName}", resource.ScopeMetrics.Count, resourceName);
            _telemetryResults.AddResourceName(resourceName);
            foreach (var scope in resource.ScopeMetrics)
            {
                _logger.LogDebug($"Received {scope.Metrics.Count} metrics for scope {scope.Scope?.Name}");
                foreach (var metric in scope.Metrics)
                {
                    if (!_telemetryResults.MetricNames.Contains(metric.Name))
                    {
                        _logger.LogDebug($"New metric seen: {metric.Name}");
                        _telemetryResults.MetricNames.Add(metric.Name);
                    }
                }
            }
        }

        var resp = new ExportMetricsServiceResponse
        {
            PartialSuccess = null
        };

        return Task.FromResult(resp);
    }
}
