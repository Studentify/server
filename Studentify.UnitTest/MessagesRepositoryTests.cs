using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Studentify.Data;
using Studentify.Data.Repositories;
using Studentify.Data.Repositories.ControllerRepositories.Implementations;
using Studentify.Models.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Studentify.Test
{
    [TestFixture]
    class MessagesRepositoryTests
    {
        private DbContextOptions<StudentifyDbContext> _dbContextOptions;
        private MessagesRepository _repository;
        private Thread _thread;
        private StudentifyDbContext _context;

        [OneTimeSetUp]
        public void Setup()
        {
            _dbContextOptions = new DbContextOptionsBuilder<StudentifyDbContext>()
                .UseInMemoryDatabase(databaseName: "StudentifyDb")
                .Options;

            _context = new StudentifyDbContext(_dbContextOptions);
            _repository = new MessagesRepository(_context,
                                                new SelectRepositoryBase<Message>(_context),
                                                new InsertRepositoryBase<Message>(_context),
                                                new UpdateRepositoryBase<Message>(_context));

            //Creating one dummy context for all messages
            _thread = new Thread { ReferencedEventId = 1 };
            _context.Threads.Add(_thread);
            _context.SaveChanges();
        }
        
        [OneTimeTearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public async Task TestPostNewMessage()
        {
            Message message = new Message()
            {
                Author = null,
                Date = DateTime.Now,
                Content = "test-content",
                IsViewed = false,
                Thread = _thread
            };

            await _repository.InsertMessageToThread(message, _thread);
            var messages = await _repository.Select.All();

            Assert.True(messages.Contains(message));
        }

        [Test]
        public async Task TestSelectAllMessagesFromThread()
        {
            var insertedMessages = new List<Message>()
            {
                new Message()
                {
                    Author = null,
                    Date = DateTime.Now,
                    Content = "test-content 1",
                    IsViewed = false,
                    Thread = _thread
                },
                new Message()
                {
                    Author = null,
                    Date = DateTime.Now,
                    Content = "test-content 2",
                    IsViewed = false,
                    Thread = _thread
                }
            };

            foreach(var message in insertedMessages)
            {
                await _repository.Insert.One(message);
            }

            var selectedMessages = await _repository.SelectAllFromThread(_thread.Id);

            Assert.True(selectedMessages.Intersect(insertedMessages).Any());
        }
    }
}
