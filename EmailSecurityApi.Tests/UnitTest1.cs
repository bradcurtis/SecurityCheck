using Xunit;
using Microsoft.EntityFrameworkCore;
using EmailSecurityApi.DAL;

namespace EmailSecurityApi.Tests
{
    public class SqlEmailRepositoryTests
    {
        private SqlEmailRepository GetRepository()
        {
            var options = new DbContextOptionsBuilder<EmailDbContext>()
                .UseInMemoryDatabase("TestDb")
                .Options;

            var context = new EmailDbContext(options);
            return new SqlEmailRepository(context);
        }

        [Fact]
        public void EmailExists_ReturnsFalse_WhenEmpty()
        {
            var repo = GetRepository();
            Assert.False(repo.EmailExists("missing@example.com"));
        }

        [Fact]
        public void AddEmail_MakesEmailExist()
        {
            var repo = GetRepository();
            repo.AddEmail("added@example.com");
            Assert.True(repo.EmailExists("added@example.com"));
        }
    }
}