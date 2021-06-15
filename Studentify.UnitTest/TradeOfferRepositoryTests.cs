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
    public class TradeOfferRepositoryTests
    {
        private DbContextOptions<StudentifyDbContext> _dbContextOptions;
        private TradeOffersRepository _repository;
        private StudentifyDbContext _context;

        [OneTimeSetUp]
        public void Setup()
        {
            _dbContextOptions = new DbContextOptionsBuilder<StudentifyDbContext>()
                .UseInMemoryDatabase(databaseName: "StudentifyDb")
                .Options;

            _context = new StudentifyDbContext(_dbContextOptions);
            _repository = new TradeOffersRepository(_context,
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
        public async Task TestInsertOneOffer()
        {
            TradeOffer tradeOffer = new TradeOffer()
            {
                AuthorId = 1,
                Address = null,
                Name = "Testowy tradeOffer insert",
                CreationDate = DateTime.Now,
                ExpiryDate = DateTime.Now,
                BuyerId = 2
            };

            await _repository.Insert.One(tradeOffer);
            var tradeOffers = await _repository.Select.All();

            Assert.True(tradeOffers.Contains(tradeOffer));
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
        public async Task TestGetOneOffer()
        {
            TradeOffer tradeOffer = new TradeOffer()
            {
                AuthorId = 1,
                Address = null,
                Name = "Testowy tradeOffer get one",
                CreationDate = DateTime.Now,
                ExpiryDate = DateTime.Now,
                BuyerId = 2
            };

            await _repository.Insert.One(tradeOffer);
            var selectedTradeOffer = await _repository.Select.ById(tradeOffer.Id);

            Assert.AreEqual(tradeOffer.Name, selectedTradeOffer.Name);
        }

        [Test]
        public async Task TestGetAllTradeOffers()
        {
            TradeOffer tradeOffer = new TradeOffer()
            {
                AuthorId = 1,
                Address = null,
                Name = "Testowy tradeOffer get all",
                CreationDate = DateTime.Now,
                ExpiryDate = DateTime.Now,
                BuyerId = 2
            };

            await _repository.Insert.One(tradeOffer);
            var tradeOffers = await _repository.Select.All();

            Assert.True(tradeOffers.ToList().Count > 0);
        }

        [Test]
        public async Task TestUpdateOneTradeOffer()
        {
            const string newTradeOfferName = "Nowa testowa oferta";

            TradeOffer tradeOffer = new TradeOffer()
            {
                AuthorId = 1,
                Address = null,
                Name = "Testowy tradeOffer insert",
                CreationDate = DateTime.Now,
                ExpiryDate = DateTime.Now,
            };

            await _repository.Insert.One(tradeOffer);

            tradeOffer.Name = newTradeOfferName;
            await _repository.Update.One(tradeOffer, tradeOffer.Id);
            var updatedTradeOffer = await _repository.Select.ById(tradeOffer.Id);

            Assert.AreEqual(newTradeOfferName, updatedTradeOffer.Name);
        }

        [Test]
        public async Task TestGetNoTradeOffers()
        {
            var tradeOffers = await _repository.Select.All();

            Assert.False(tradeOffers.Any(m => m.Name == "There is no tradeOffer"));
        }
    }
}