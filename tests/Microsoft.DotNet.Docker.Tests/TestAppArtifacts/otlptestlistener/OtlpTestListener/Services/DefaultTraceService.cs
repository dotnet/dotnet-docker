namespace OtlpTestListener.Services;

public class DefaultTraceService : TraceService.TraceServiceBase
{
    private readonly ILogger<DefaultTraceService> _logger;
    private readonly TelemetryResults _telemetryResults;

    public DefaultTraceService(ILogger<DefaultTraceService> logger, TelemetryResults telemetryResults)
    {
        _logger = logger;
        _telemetryResults = telemetryResults;
    }

    public override Task<ExportTraceServiceResponse> Export(ExportTraceServiceRequest request, ServerCallContext context)
    {
        foreach (var resource in request.ResourceSpans)
        {
            var resourceName = resource.Resource?.Attributes.FirstOrDefault(a => a.Key == OtlpApplication.SERVICE_NAME)?.Value.ValueString();
            _logger.LogDebug("Received {count} scope spans for resource {resourceName}", resource.ScopeSpans.Count, resourceName);
            _telemetryResults.AddResourceName(resourceName);
            
            foreach (var scope in resource.ScopeSpans)
            {
                _logger.LogDebug($"Received {scope.Spans.Count} spans for scope {scope.Scope?.Name}");
                foreach (var span in scope.Spans)
                {
                    var TraceId = span.TraceId.ToHexString();
                    if (!_telemetryResults.TraceIds.Contains(TraceId))
                    {
                        _logger.LogDebug($"New TraceId seen: {TraceId}");
                        _telemetryResults.TraceIds.Add(TraceId);
                    }
                    _telemetryResults.SpanIdCount += 1;
                }
            }
        }

        var resp = new ExportTraceServiceResponse
        {
            PartialSuccess = null
        };
        return Task.FromResult(resp);
    }
}
