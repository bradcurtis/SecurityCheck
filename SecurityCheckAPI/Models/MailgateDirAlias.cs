namespace EmailSecurityApi.Models
{
    public class MailgateDirAlias
    {
        public int Id { get; set; }
        public string EmailAddress { get; set; } = string.Empty;
    }

    public class PORTTLSDomain
    {
        public int Id { get; set; }
        public string DomainName { get; set; } = string.Empty;
    }
}