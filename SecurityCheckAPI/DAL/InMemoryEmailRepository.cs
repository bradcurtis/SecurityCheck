using System.Collections.Concurrent;

namespace EmailSecurityApi.DAL
{
    public class InMemoryEmailRepository : IEmailRepository
    {
        private readonly ConcurrentDictionary<string, bool> _emails = new();

        public InMemoryEmailRepository()
        {
            // Seed with sample data
            _emails.TryAdd("test@example.com", true);
            _emails.TryAdd("hello@world.net", true);
        }

        public bool EmailExists(string email)
        {
            return _emails.ContainsKey(email.ToLowerInvariant());
        }

        public void AddEmail(string email)
        {
            _emails.TryAdd(email.ToLowerInvariant(), true);
        }

        public IEnumerable<string> GetAllEmails()
        {
            return _emails.Keys;
        }
    }
}