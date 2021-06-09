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
    class StudentifyAccountsRepositoryTests
    {
        private DbContextOptions<StudentifyDbContext> _dbContextOptions;
        private StudentifyAccountsRepository _repository;

        [OneTimeSetUp]
        public void Setup()
        {
            _dbContextOptions = new DbContextOptionsBuilder<StudentifyDbContext>()
                .UseInMemoryDatabase(databaseName: "StudentifyDb")
                .Options;

            var context = new StudentifyDbContext(_dbContextOptions);
            _repository = new StudentifyAccountsRepository(context,
                                new SelectRepositoryBase<StudentifyAccount>(context),
                                new UpdateRepositoryBase<StudentifyAccount>(context),
                                new InsertRepositoryBase<StudentifyAccount>(context));
        }

        [Test]
        public async Task CreateAccountSuccess()
        {
            const int expectedNumberOfAccounts = 1;

            StudentifyUser user = new StudentifyUser()
            {
                UserName = "test-user",
            };

            await _repository.InsertFromStudentifyUser(user);

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

            await _repository.InsertFromStudentifyUser(user);

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
            await _repository.InsertFromStudentifyUser(user);

            var account = await _repository.SelectByUsername("skilled-user");
            await _repository.SaveSkill(account.Id, skill);

            var skills = await _repository.GetSkills(account.Id);
            Assert.IsTrue(skills.Contains(skill));
        }
    }
}