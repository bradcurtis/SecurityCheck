using Microsoft.EntityFrameworkCore;

namespace EmailSecurityApi.DAL
{
    public class EmailDbContext : DbContext
    {
        public EmailDbContext(DbContextOptions<EmailDbContext> options) : base(options) { }

        public DbSet<EmailEntity> Emails { get; set; }
    }
}