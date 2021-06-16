using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Studentify.Data;
using Studentify.Data.Repositories;
using Studentify.Data.Repositories.ControllerRepositories.Implementations;
using Studentify.Models;
using Studentify.Models.Authentication;
using Studentify.Models.StudentifyEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Studentify.Test
{
    class InfoRepositoryTests
    {
        private DbContextOptions<StudentifyDbContext> _dbContextOptions;
        private InfosRepository _repository;
        private StudentifyDbContext _context;

        [OneTimeSetUp]
        public void Setup()
        {
            _dbContextOptions = new DbContextOptionsBuilder<StudentifyDbContext>()
                .UseInMemoryDatabase(databaseName: "StudentifyDb")
                .Options;

            _context = new StudentifyDbContext(_dbContextOptions);
            _repository = new InfosRepository(_context,
                                                 new SelectRepositoryBase<Info>(_context),
                                                 new InsertRepositoryBase<Info>(_context),
                                                 new UpdateRepositoryBase<Info>(_context));
        }
        
        [OneTimeTearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public async Task TestInsertOneInfoAlert()
        {
            Info info = new Info()
            {
                AuthorId = 1,
                Address = null,
                Name = "Testowy info-alert insert",
                CreationDate = DateTime.Now,
                ExpiryDate = DateTime.Now,
                Category = InfoCategory.Alert
            };

            await _repository.Insert.One(info);
            var infos = await _repository.Select.All();

            Assert.True(infos.Contains(info));
        }

        [Test]
        public async Task TestGetOneInfoNotice()
        {
            Info info = new Info()
            {
                AuthorId = 1,
                Address = null,
                Name = "Testowy info-notice get one",
                CreationDate = DateTime.Now,
                ExpiryDate = DateTime.Now,
                Category = InfoCategory.Notice
            };

            await _repository.Insert.One(info);
            var selectedInfo = await _repository.Select.ById(info.Id);

            Assert.AreEqual(info.Name, selectedInfo.Name);
        }

        [Test]
        public async Task TestRaiseWhenWrongSelect()
        {
            try
            {
                await _repository.Select.ById(-2);
                Assert.Fail("Expected exception, but didnt get any");
            }
            catch (System.Data.DataException)
            {}
        }

        [Test]
        public async Task TestGetAllInfos()
        {
            Info info = new Info()
            {
                AuthorId = 1,
                Address = null,
                Name = "Testowe info get all",
                CreationDate = DateTime.Now,
                ExpiryDate = DateTime.Now,
                Category = InfoCategory.Alert
            };

            await _repository.Insert.One(info);
            var infos = await _repository.Select.All();

            Assert.True(infos.ToList().Count > 0);
        }


        [Test]
        public async Task TestUpdateOneInfo()
        {
            const string newInfoName = "Nowe testowe spotkanie";

            Info info = new Info()
            {
                AuthorId = 1,
                Address = null,
                Name = "Testowe info-alert insert",
                CreationDate = DateTime.Now,
                ExpiryDate = DateTime.Now,
                Category = InfoCategory.Alert
            };

            await _repository.Insert.One(info);

            info.Name = newInfoName;
            await _repository.Update.One(info, info.Id);
            var updatedInfo = await _repository.Select.ById(info.Id);

            Assert.AreEqual(newInfoName, updatedInfo.Name);
        }

        [Test]
        public async Task TestGetNoInfos()
        {
            var infos = await _repository.Select.All();

            Assert.False(infos.Any(m => m.Name == "There is no info"));
        }
    }
}
