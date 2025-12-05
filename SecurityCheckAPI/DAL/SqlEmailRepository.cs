using System.Collections.Generic;
using System.Linq;

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
            return _context.Emails.Any(e => e.Address == email);
        }

        public void AddEmail(string email)
        {
            _context.Emails.Add(new EmailEntity { Address = email });
            _context.SaveChanges();
        }

        public IEnumerable<string> GetAllEmails()
        {
            return _context.Emails.Select(e => e.Address).ToList();
        }
    }
}