using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Studentify.Data;
using Studentify.Data.Repositories;
using Studentify.Data.Repositories.ControllerRepositories.Implementations;
using Studentify.Models.Messages;
using System.Linq;
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
                ReferencedEventId = 1,
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
                ReferencedEventId = 1,
            };

            await _repository.Insert.One(insertedThread);
            var selectedThread = await _repository.Select.ById(insertedThread.Id);

            Assert.AreEqual(insertedThread, selectedThread);
        }

        [Test]
        public async Task TestSelectAllThreads()
        {
            Thread insertedThread = new Thread()
            {
                ReferencedEventId = 1,
            };

            await _repository.Insert.One(insertedThread);
            var threads = await _repository.Select.All();

            Assert.True(threads.Any());
        }

        [Test]
        public async Task TestUpdateOneThread()
        {
            const int newEventId = 2;

            Thread insertedThread = new Thread()
            {
                ReferencedEventId = 1,
            };

            await _repository.Insert.One(insertedThread);

            insertedThread.ReferencedEventId = newEventId;
            await _repository.Update.One(insertedThread, insertedThread.Id);
            var selectedThread = await _repository.Select.ById(insertedThread.Id);

            Assert.AreEqual(newEventId, selectedThread.ReferencedEventId);
        }
    }
}
