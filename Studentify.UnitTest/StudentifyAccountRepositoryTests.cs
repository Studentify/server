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
    class StudentifyAccountRepositoryTests
    {
        private DbContextOptions<StudentifyDbContext> _dbContextOptions;
        private StudentifyAccountsRepository _accountsRepository;

        [OneTimeSetUp]
        public async void Setup()
        {
            _dbContextOptions = new DbContextOptionsBuilder<StudentifyDbContext>()
                .UseInMemoryDatabase(databaseName: "StudentifyDb")
                .Options;
            await using var context = new StudentifyDbContext(_dbContextOptions);
            _accountsRepository = new StudentifyAccountsRepository(context,
                new SelectRepositoryBase<StudentifyAccount>(context),
                new UpdateRepositoryBase<StudentifyAccount>(context),
                new InsertRepositoryBase<StudentifyAccount>(context));
        }

        [Test]
        public async Task ShouldSucceedWhenCreatingNewAccount()
        {
            const int expectedNumberOfAccounts = 1;

            StudentifyUser user = new StudentifyUser()
            {
                UserName = "test-user",
            };

            var account = new StudentifyAccount{StudentifyUserId = user.Id, User = user};
            await _accountsRepository.Insert.One(account);

            var accounts = (await _accountsRepository.Select.All())
                .Where(acc => acc.StudentifyUserId == user.Id).ToList();
            
            Assert.AreEqual(expectedNumberOfAccounts, accounts.Count);
        }
    }
}