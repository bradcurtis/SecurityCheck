using System.Collections.Generic;
using System.Linq;

namespace EmailSecurityApi.DAL
{
    public class MockEmailRepository : IEmailRepository
    {
        private readonly HashSet<string> _aliases;
        private readonly HashSet<string> _domains;

        public MockEmailRepository()
        {
            // Example secured aliases
            _aliases = new HashSet<string>
            {
                "alice@contoso.com",
                "bob@fabrikam.com",
                "ceo@securemail.org"
            };

            // Example secured domains
            _domains = new HashSet<string>
            {
                "contoso.com",
                "fabrikam.com",
                "trusted.org"
            };
        }

        public bool EmailExists(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            var normalizedEmail = email.Trim().ToLowerInvariant();

            // Extract domain
            var atIndex = normalizedEmail.IndexOf('@');
            var domain = atIndex > -1 ? normalizedEmail[(atIndex + 1)..] : string.Empty;

            // Check alias list
            bool aliasMatch = _aliases.Contains(normalizedEmail);

            // Check domain list
            bool domainMatch = !string.IsNullOrEmpty(domain) && _domains.Contains(domain);

            return aliasMatch || domainMatch;
        }
    }
}