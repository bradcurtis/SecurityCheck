using Microsoft.AspNetCore.Mvc;
using EmailSecurityApi.Models;
using EmailSecurityApi.DAL;
using EmailSecurityApi.Services;

namespace EmailSecurityApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmailController : ControllerBase
    {
        private readonly IEmailRepository _repository;
        private readonly ApiLogger _logger;

        public EmailController(IEmailRepository repository, ApiLogger logger)
        {
            _repository = repository;
            _logger = logger;
        }

        // --- Single email endpoint (backward compatibility) ---
        [HttpPost("check")]
        public ActionResult<EmailResponse> CheckEmail([FromBody] EmailRequest request)
        {
            _logger.LogInfo($"Checking single email: {request.Email}");

            bool exists = _repository.EmailExists(request.Email);

            var response = new EmailResponse
            {
                Email = request.Email,
                Secured = exists
            };

            return Ok(response);
        }

        // --- Batch email endpoint (new) ---
        [HttpPost("check-multiple")]
        public ActionResult<EmailBatchResponse> CheckEmails([FromBody] EmailBatchRequest request)
        {
            _logger.LogInfo($"Checking {request.Emails.Count} emails");

            var results = request.Emails.Select(email => new EmailCheckResult
            {
                Email = email,
                Secured = _repository.EmailExists(email)
            }).ToList();

            return Ok(new EmailBatchResponse { Results = results });
        }
    }
}