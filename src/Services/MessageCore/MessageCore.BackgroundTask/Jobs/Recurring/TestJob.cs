using MessageCore.BackgroundTask.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace MessageCore.BackgroundTask.Jobs.Recurring
{
    public class TestJob
    {
        private readonly ILogger<TestJob> _logger;

        public TestJob(ILogger<TestJob> logger)
        {
            _logger = logger;
        }

        public async Task ExecuteAsync()
        {
            await Task.Delay(100);
            _logger.LogTrace("LogTrace");
            _logger.LogDebug("LogDebug");
            _logger.LogInformation("LogInformation");
            _logger.LogWarning("LogWarning");
            _logger.LogError("LogError");
            _logger.LogCritical("LogCritical");

            //throw new Exception("TestJob for test new exception");
        }
    }
}
