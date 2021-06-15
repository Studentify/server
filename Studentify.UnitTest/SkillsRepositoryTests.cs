using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Studentify.Data;
using Studentify.Data.Repositories;
using Studentify.Data.Repositories.ControllerRepositories.Implementations;
using Studentify.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Studentify.Test
{
    [TestFixture]
    class SkillsRepositoryTests
    {
        private DbContextOptions<StudentifyDbContext> _dbContextOptions;
        private SkillsRepository _repository;
        private StudentifyDbContext _context;

        [OneTimeSetUp]
        public void Setup()
        {
            _dbContextOptions = new DbContextOptionsBuilder<StudentifyDbContext>()
                .UseInMemoryDatabase(databaseName: "StudentifyDb")
                .Options;

            _context = new StudentifyDbContext(_dbContextOptions);
            _repository = new SkillsRepository(_context,
                                               new SelectRepositoryBase<Skill>(_context),
                                               new InsertRepositoryBase<Skill>(_context),
                                               new UpdateRepositoryBase<Skill>(_context));
        }
        
        [OneTimeTearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public async Task TestPostNewSkill()
        {
            Skill skill = new Skill()
            {
                Name = "Skill",
                Owner = null,
                Rate = 50
            };

            await _repository.Insert.One(skill);
            var skills = await _repository.Select.All();

            Assert.True(skills.Contains(skill));
        }

        [Test]
        public async Task TestUpdateSkillRate()
        {
            const int newSkillRate = 70;

            Skill skill = new Skill()
            {
                Name = "Skill-to-update",
                Owner = null,
                Rate = 50
            };

            await _repository.Insert.One(skill);
            var selectedSkill = (await _repository.Select.All())
                        .Where(s => s.Name == "Skill-to-update")
                        .FirstOrDefault();

            await _repository.UpdateSkillRate(selectedSkill.Id, newSkillRate);

            Assert.AreEqual(newSkillRate, selectedSkill.Rate);
        }
    }
}
