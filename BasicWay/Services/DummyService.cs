﻿using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace BasicWay.Services
{
    public class DummyService : IDummyService
    {
        private readonly ILogger _logger;

        public DummyService(ILogger logger)
        {
            _logger = logger;
        }

        public Task DoNothingAsync()
        {
            LogToAllLevels(nameof(DoNothingAsync));
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
    }
}