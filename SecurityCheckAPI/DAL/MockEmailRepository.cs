
namespace EmailSecurityApi.DAL
{
  /*  public interface IEmailRepository
    {
        bool EmailExists(string email);
    }*/

    // Mock implementation for testing
    public class MockEmailRepository : IEmailRepository
    {
        private readonly HashSet<string> _emails = new HashSet<string>
        {
            "test@example.com",
            "user@domain.com"
        };

        public void AddEmail(string email)
        {
            throw new NotImplementedException();
        }

        public bool EmailExists(string email)
        {
            return _emails.Contains(email.ToLower());
        }

        public IEnumerable<string> GetAllEmails()
        {
            throw new NotImplementedException();
        }
    }
}