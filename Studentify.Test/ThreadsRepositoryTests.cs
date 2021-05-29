using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Studentify.Data;
using Studentify.Data.Repositories;
using Studentify.Data.Repositories.ControllerRepositories.Implementations;
using Studentify.Models.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Studentify.Test
{
    [TestFixture]
    class ThreadsRepositoryTests
    {
        private DbContextOptions<StudentifyDbContext> _dbContextOptions;
        private ThreadsRepository _repository;

        [OneTimeSetUp]
        public void Setup()
        {
            _dbContextOptions = new DbContextOptionsBuilder<StudentifyDbContext>()
                .UseInMemoryDatabase(databaseName: "StudentifyDb")
                .Options;

            StudentifyDbContext context = new StudentifyDbContext(_dbContextOptions);
            _repository = new ThreadsRepository(context,
                                                new SelectRepositoryBase<Thread>(context),
                                                new InsertRepositoryBase<Thread>(context),
                                                new UpdateRepositoryBase<Thread>(context));
        }

        [Test]
        public async Task TestInsertOneThread()
        {
            Thread thread = new Thread()
            {
                EventId = 1,
            };

            await _repository.Insert.One(thread);
            var threads = await _repository.Select.All();

            Assert.True(threads.Contains(thread));
        }

        [Test]
        public async Task TestSelectOneThread()
        {
            Thread insertedThread = new Thread()
            {
                EventId = 1,
            };

            await _repository.Insert.One(insertedThread);
            var selectedThread = await _repository.Select.ById(insertedThread.Id);

            Assert.AreEqual(insertedThread, selectedThread);
        }
    }
}
