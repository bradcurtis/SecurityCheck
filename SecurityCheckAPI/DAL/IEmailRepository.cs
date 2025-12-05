namespace EmailSecurityApi.DAL
{
    public interface IEmailRepository
    {
        bool EmailExists(string email);
        void AddEmail(string email);
        IEnumerable<string> GetAllEmails();
    }
}
