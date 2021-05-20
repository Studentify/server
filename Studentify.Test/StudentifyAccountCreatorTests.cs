using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Studentify.Data;
using Studentify.Models;
using Studentify.Models.Authentication;

namespace Studentify.Test
{
    [TestFixture]
    class StudentifyAccountCreatorTests
    {
        private DbContextOptions<StudentifyDbContext> _dbContextOptions;

        [OneTimeSetUp]
        public void Setup()
        {
            _dbContextOptions = new DbContextOptionsBuilder<StudentifyDbContext>()
                                    .UseInMemoryDatabase(databaseName: "StudentifyDb")
                                    .Options;
        }

        [Test]
        public async Task CreateAccountSuccess()
        {
            await using var context = new StudentifyDbContext(_dbContextOptions);
            StudentifyAccountCreator creator = new StudentifyAccountCreator(context);
            const int expectedNumberOfAccounts = 1;

            StudentifyUser user = new StudentifyUser()
            {
                UserName = "test-user",
            };

            var response = creator.CreateAccount(user);
            StudentifyAccount[] accounts = await context.StudentifyAccounts
                                                        .Where(acc => acc.StudentifyUserId == user.Id)
                                                        .ToArrayAsync();

            Assert.AreEqual("Success", response.Status);
            Assert.AreEqual(expectedNumberOfAccounts, accounts.Length);
        }
    }
}
