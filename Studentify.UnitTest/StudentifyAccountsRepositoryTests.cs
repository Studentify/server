using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Studentify.Data;
using Studentify.Data.Repositories;
using Studentify.Models;
using Studentify.Models.Authentication;

namespace Studentify.Test
{
    [TestFixture, Order(0)]
    class StudentifyAccountsRepositoryTests
    {
        private DbContextOptions<StudentifyDbContext> _dbContextOptions;
        private StudentifyAccountsRepository _repository;
        private StudentifyDbContext _context;

        [OneTimeSetUp]
        public void Setup()
        {
            _dbContextOptions = new DbContextOptionsBuilder<StudentifyDbContext>()
                .UseInMemoryDatabase(databaseName: "StudentifyDb")
                .Options;

            _context = new StudentifyDbContext(_dbContextOptions);
            _repository = new StudentifyAccountsRepository(_context,
                                new SelectRepositoryBase<StudentifyAccount>(_context),
                                new UpdateRepositoryBase<StudentifyAccount>(_context),
                                new InsertRepositoryBase<StudentifyAccount>(_context));
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public async Task CreateAccountSuccess()
        {
            const int expectedNumberOfAccounts = 1;

            StudentifyUser user = new StudentifyUser()
            {
                UserName = "test-user",
            };
            
            var account = new StudentifyAccount{StudentifyUserId = user.Id, User = user};
            await _repository.Insert.One(account);

            var accounts = (await _repository.Select.All())
                .Where(acc => acc.StudentifyUserId == user.Id).ToList();
            
            Assert.AreEqual(expectedNumberOfAccounts, accounts.Count);
        }

        [Test]
        public async Task TestSelectByUsername()
        {       
            StudentifyUser user = new StudentifyUser()
            {
                UserName = "test-user",
            };

            var addedAccount = new StudentifyAccount{StudentifyUserId = user.Id, User = user};
            await _repository.Insert.One(addedAccount);

            var account = await _repository.SelectByUsername("test-user");

            Assert.IsTrue(account != null);
        }

        [Test]
        public async Task TestSkillsManagement()
        {
            StudentifyUser user = new StudentifyUser()
            {
                UserName = "skilled-user",
            };

            Skill skill = new Skill()
            {
                Name = "skill",
                Owner = null,
                Rate = 50
            };

            var addedAccount = new StudentifyAccount{StudentifyUserId = user.Id, User = user};
            await _repository.Insert.One(addedAccount);

            var account = await _repository.SelectByUsername("skilled-user");
            await _repository.SaveSkill(account.Id, skill);

            var skills = await _repository.GetSkills(account.Id);
            Assert.IsTrue(skills.Contains(skill));
        }
    }
}