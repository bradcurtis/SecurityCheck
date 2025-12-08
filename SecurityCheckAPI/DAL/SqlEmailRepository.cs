using EmailSecurityApi.Models;

namespace EmailSecurityApi.DAL
{
    public class SqlEmailRepository : IEmailRepository
    {
        private readonly EmailDbContext _context;

        public SqlEmailRepository(EmailDbContext context)
        {
            _context = context;
        }

        public bool EmailExists(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            var normalizedEmail = email.Trim().ToLowerInvariant();
            var atIndex = normalizedEmail.IndexOf('@');
            var domain = atIndex > -1 ? normalizedEmail[(atIndex + 1)..] : string.Empty;

            bool aliasMatch = _context.MailgateDirAlias
                .Any(a => a.EmailAddress.ToLower() == normalizedEmail);

            bool domainMatch = !string.IsNullOrEmpty(domain) &&
                _context.PORTTLSDomains
                .Any(d => d.DomainName.ToLower() == domain);

            return aliasMatch || domainMatch;
        }

        // ✅ AddEmail implementation
        public void AddEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return;

            var normalizedEmail = email.Trim().ToLowerInvariant();

            _context.MailgateDirAlias.Add(new MailgateDirAlias
            {
                EmailAddress = normalizedEmail
            });

            _context.SaveChanges();
        }
    }
}