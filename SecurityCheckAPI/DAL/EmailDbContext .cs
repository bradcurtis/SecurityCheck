using EmailSecurityApi.Models;
using Microsoft.EntityFrameworkCore;

namespace EmailSecurityApi.DAL
{
    public class EmailDbContext : DbContext
    {
        public EmailDbContext(DbContextOptions<EmailDbContext> options) : base(options) { }

        public DbSet<MailgateDirAlias> MailgateDirAlias { get; set; }
        public DbSet<PORTTLSDomain> PORTTLSDomains { get; set; }
    }
}