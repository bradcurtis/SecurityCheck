namespace EmailSecurityApi.DAL
{
    public interface IEmailRepository
    {
        bool EmailExists(string email);

        // Add a new email alias into the repository
        void AddEmail(string email);
    }
}