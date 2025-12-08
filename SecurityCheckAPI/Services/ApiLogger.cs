using Microsoft.Extensions.Logging;

namespace EmailSecurityApi.Services
{
    /// <summary>
    /// Centralized logging wrapper for the EmailSecurityApi.
    /// Provides convenience methods for common log levels and structured logging.
    /// </summary>
    public class ApiLogger
    {
        private readonly ILogger<ApiLogger> _logger;

        public ApiLogger(ILogger<ApiLogger> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Logs an informational message.
        /// </summary>
        public void LogInfo(string message)
        {
            _logger.LogInformation(message);
        }

        /// <summary>
        /// Logs an informational message with structured data.
        /// </summary>
        public void LogInfo(string message, object data)
        {
            _logger.LogInformation("{Message} | Data: {@Data}", message, data);
        }

        /// <summary>
        /// Logs a debug message.
        /// </summary>
        public void LogDebug(string message)
        {
            _logger.LogDebug(message);
        }

        /// <summary>
        /// Logs a warning message.
        /// </summary>
        public void LogWarning(string message)
        {
            _logger.LogWarning(message);
        }

        /// <summary>
        /// Logs an error message.
        /// </summary>
        public void LogError(string message)
        {
            _logger.LogError(message);
        }

        /// <summary>
        /// Logs an error message with exception details.
        /// </summary>
        public void LogError(string message, Exception ex)
        {
            _logger.LogError(ex, message);
        }

        /// <summary>
        /// Logs a critical error message.
        /// </summary>
        public void LogCritical(string message)
        {
            _logger.LogCritical(message);
        }

        /// <summary>
        /// Logs a critical error message with exception details.
        /// </summary>
        public void LogCritical(string message, Exception ex)
        {
            _logger.LogCritical(ex, message);
        }
    }
}