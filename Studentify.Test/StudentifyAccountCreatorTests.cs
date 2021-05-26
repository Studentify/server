using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Studentify.Data;
using Studentify.Data.Repositories;
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
            var accountsRepository = new StudentifyAccountsRepository(context,
                new SelectRepositoryBase<StudentifyAccount>(context),
                new UpdateRepositoryBase<StudentifyAccount>(context),
                new InsertRepositoryBase<StudentifyAccount>(context));

            const int expectedNumberOfAccounts = 1;

            StudentifyUser user = new StudentifyUser()
            {
                UserName = "test-user",
            };

            await accountsRepository.InsertFromStudentifyUser(user);

            var accounts = (await accountsRepository.Select.All())
                .Where(acc => acc.StudentifyUserId == user.Id).ToList();
            
            Assert.AreEqual(expectedNumberOfAccounts, accounts.Count);
        }
    }
}