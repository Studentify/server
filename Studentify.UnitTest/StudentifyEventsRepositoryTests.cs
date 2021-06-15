using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Studentify.Data;
using Studentify.Data.Repositories;
using Studentify.Models.StudentifyEvents;

namespace Studentify.Test
{
    [TestFixture]
    public class StudentifyEventsRepositoryTests
    {
        private DbContextOptions<StudentifyDbContext> _dbContextOptions;
        private StudentifyEventsRepository _repository;
        private StudentifyDbContext _context;
        private TradeOffersRepository _helperRepository;

        [OneTimeSetUp]
        public void Setup()
        {
            _dbContextOptions = new DbContextOptionsBuilder<StudentifyDbContext>()
                .UseInMemoryDatabase(databaseName: "StudentifyDb")
                .Options;

            _context = new StudentifyDbContext(_dbContextOptions);
            _repository = new StudentifyEventsRepository(_context,
                                                 new SelectRepositoryBase<StudentifyEvent>(_context),
                                                 new DeleteRepositoryBase<StudentifyEvent>(_context));
            
            _helperRepository = new TradeOffersRepository(_context,
                                                new SelectRepositoryBase<TradeOffer>(_context),
                                                new InsertRepositoryBase<TradeOffer>(_context),
                                                new UpdateRepositoryBase<TradeOffer>(_context));
        }
        
        [OneTimeTearDown]
        public void TearDown()
        {
            _context.Dispose();
        }
        
        [Test]
        public async Task TestGetEvent()
        {
            TradeOffer studentifyEvent = new TradeOffer()
            {
                AuthorId = 1,
                Address = null,
                Name = "Testowy studentifyEvent get one",
                CreationDate = DateTime.Now,
                ExpiryDate = DateTime.Now,
            };

            await _helperRepository.Insert.One(studentifyEvent);
            var selectedTradeOffer = await _repository.Select.ById(studentifyEvent.Id);

            Assert.AreEqual(studentifyEvent.Name, selectedTradeOffer.Name);
        }

        [Test]
        public async Task TestGetAllStudentifyEvents()
        {
            var studentifyEvents = await _repository.Select.All();
            var count = studentifyEvents.ToList().Count;
            
            TradeOffer studentifyEvent = new TradeOffer()
            {
                Name = "Testowy studentifyEvent get all",
                CreationDate = DateTime.Now,
                ExpiryDate = DateTime.Now,
            };

            await _helperRepository.Insert.One(studentifyEvent);
            studentifyEvents = await _repository.Select.All();

            Assert.True(studentifyEvents.ToList().Count > count);
        }
        

        [Test]
        public async Task TestGetNoStudentifyEvents()
        {
            var studentifyEvents = await _repository.Select.All();

            Assert.False(studentifyEvents.Any(m => m.Name == "There is no studentifyEvent"));
        }


        [Test]
        public async Task TestDeleteEvent()
        {
            TradeOffer studentifyEvent = new TradeOffer()
            {
                Name = "Testowy studentifyEvent get all",
                CreationDate = DateTime.Now,
                ExpiryDate = DateTime.Now,
            };
            
            await _helperRepository.Insert.One(studentifyEvent);
            var studentifyEvents = await _repository.Select.All();
            Assert.True(studentifyEvents.ToList().Count > 0);
            
            var count = studentifyEvents.ToList().Count;
            await _repository.Delete.ById(studentifyEvent.Id);
            studentifyEvents = await _repository.Select.All();
            Assert.True(studentifyEvents.ToList().Count == count - 1);
        }
    }
}