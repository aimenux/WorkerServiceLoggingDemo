using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility.Implementation;
using Microsoft.Extensions.Logging;

namespace AdvancedWay.Services;

public class DummyService : IDummyService
{
    private readonly ILogger<DummyService> _logger;
    private readonly TelemetryClient _telemetryClient;

    public DummyService(ILogger<DummyService> logger, TelemetryClient telemetryClient)
    {
        _logger = logger;
        _telemetryClient = telemetryClient;
    }

    public Task DoNothingAsync()
    {
        LogToAllLevels(nameof(DoNothingAsync));
        LogToAllLevels<RequestTelemetry>(nameof(DoNothingAsync));
        return Task.CompletedTask;
    }

    private void LogToAllLevels(string message)
    {
        var scope = $"Scope-{GetType().Namespace}";
        using(_logger.BeginScope(scope))
        {
            _logger.LogTrace(message);
            _logger.LogDebug(message);
            _logger.LogInformation(message);
            _logger.LogWarning(message);
            _logger.LogError(message);
            _logger.LogCritical(message);
        }
    }

    private void LogToAllLevels<T>(string message) where T : OperationTelemetry, new()
    {
        var operation = $"Operation-{GetType().Namespace}";
        using (_telemetryClient.StartOperation<T>(operation))
        {
            _logger.LogTrace(message);
            _logger.LogDebug(message);
            _logger.LogInformation(message);
            _logger.LogWarning(message);
            _logger.LogError(message);
            _logger.LogCritical(message);
        }
    }
}