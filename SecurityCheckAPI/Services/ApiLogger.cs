using Microsoft.Extensions.Logging;

namespace EmailSecurityApi.Services
{
    public class ApiLogger
    {
        private readonly ILogger<ApiLogger> _logger;

        public ApiLogger(ILogger<ApiLogger> logger)
        {
            _logger = logger;
        }

        public void LogInfo(string message)
        {
            _logger.LogInformation(message);
        }

        public void LogError(string message)
        {
            _logger.LogError(message);
        }
    }
}