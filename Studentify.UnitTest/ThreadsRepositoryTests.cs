using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Studentify.Data;
using Studentify.Data.Repositories;
using Studentify.Data.Repositories.ControllerRepositories.Implementations;
using Studentify.Models;
using Studentify.Models.Authentication;
using Studentify.Models.Messages;
using Studentify.Models.StudentifyEvents;
using System.Linq;
using System.Threading.Tasks;

namespace Studentify.Test
{
    [TestFixture, Order(0)]
    class ThreadsRepositoryTests
    {
        private DbContextOptions<StudentifyDbContext> _dbContextOptions;
        private ThreadsRepository _repository;
        private StudentifyDbContext _context;

        [OneTimeSetUp]
        public void Setup()
        {
            _dbContextOptions = new DbContextOptionsBuilder<StudentifyDbContext>()
                .UseInMemoryDatabase(databaseName: "StudentifyDb")
                .Options;

            _context = new StudentifyDbContext(_dbContextOptions);
            _repository = new ThreadsRepository(_context,
                                                new SelectRepositoryBase<Thread>(_context),
                                                new InsertRepositoryBase<Thread>(_context),
                                                new UpdateRepositoryBase<Thread>(_context));
        }
        
        [OneTimeTearDown]
        public void TearDown()
        {
            _context.Dispose();
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

        [Test]
        public async Task TestGetThreadsForSpecifiedUser()
        {
            StudentifyAccountsRepository studentifyAccountsRepository = new StudentifyAccountsRepository(_context,
                                                                        new SelectRepositoryBase<StudentifyAccount>(_context),
                                                                        new UpdateRepositoryBase<StudentifyAccount>(_context),
                                                                        new InsertRepositoryBase<StudentifyAccount>(_context));
            StudentifyUser user = new StudentifyUser
            {
                UserName = "test-user-threader"
            };
            StudentifyAccount account = new StudentifyAccount
            {
                StudentifyUserId = user.Id,
                User = user
            };
            Thread insertedThread = new Thread()
            {
                ReferencedEventId = 1,
                ReferencedEvent = new Info { Author = account },
                UserAccount = account
            };

            await studentifyAccountsRepository.Insert.One(account);
            await _repository.Insert.One(insertedThread);
            var threads = await _repository.SelectAllUserRelatedThreads(account.UserName);
            Assert.True(threads.Any());
        }
    }
}
