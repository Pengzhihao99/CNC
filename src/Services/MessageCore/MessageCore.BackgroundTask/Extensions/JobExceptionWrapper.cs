using MessageCore.Infrastructure.Exceptions;

namespace MessageCore.BackgroundTask.Extensions
{
    public class JobExceptionWrapper
    {
        private readonly ILogger<JobExceptionWrapper> _logger;
        public JobExceptionWrapper(ILogger<JobExceptionWrapper> logger)
        {
            _logger = logger;
        }
        public async Task InvokeAsync(Func<Task> func, string jobName)
        {
            try
            {
                _logger.LogInformation($"{jobName} Execute");
                await func();
            }
            catch (MessageCoreInternalException ex)
            {
                _logger.LogWarning($"{jobName} {ex.Type} {ex.ToString()}");
            }
            catch (MessageCoreExternalException ex)
            {
                _logger.LogWarning($"{jobName} {ex.Type} {ex.ToString()}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"{jobName} {ExceptionType.MessageCoreUnhandledException} {ex.ToString()}");
                throw;
            }
        }
    }
}
