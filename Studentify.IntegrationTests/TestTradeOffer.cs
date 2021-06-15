using System;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Mime;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Linq;

using NUnit.Framework;
using Studentify.Models.HttpBody;
using Studentify.Models;
using Studentify.IntegrationTests.GetDto;

namespace Studentify.IntegrationTests
{
    class TestTradeOffer
    {
        private readonly AppFactory<Startup> _factory = new();

        private HttpClient _clientBuyer;
        private LoginGetDto _loginDataBuyer;
        private readonly RegisterDto _testUserBuyer = new()
        {
            Username = "Testiman_trade_buyer",
            Email = "test_trade_buyer@test.test",
            FirstName = "Test",
            LastName = "Testowsky",
            Password = "Test123!"
        };

        private HttpClient _clientSeller;
        private LoginGetDto _loginDataSeller;
        private readonly RegisterDto _testUserSeller = new()
        {
            Username = "Testiman_trade_seller",
            Email = "test_trade_seller@test.test",
            FirstName = "Test",
            LastName = "Testowsky",
            Password = "Test123!"
        };

        private TradeOfferDto _testOffer = new()
        {
            Name = "Test Trade Offer",
            ExpiryDate = new DateTime(2030, 1, 1, 12, 0, 0),
            Description = "This is an exceptionally good trade opportunity",
            Longitude = 51.688117,
            Latitude = 12.002481,
            Address = new Address()
            {
                Country = "Polska",
                Town = "Lipinki Łużyckie",
                PostalCode = "68-213",
                Street = "Łączna",
                HouseNumber = "43"
            },
            Offer = "Nothing",
            Price = "A soul",
            BuyerId = null
        };
        private TradeOfferGetDto _testOfferGetDto;

        [OneTimeSetUp]
        public async Task PrepareAccountAndClient()
        {
            _clientBuyer = _factory.CreateClient();
            await Utilities.Register(_clientBuyer, _testUserBuyer);
            _loginDataBuyer = await Utilities.Login(_clientBuyer, new LoginDto
            {
                Username = _testUserBuyer.Username,
                Password = _testUserBuyer.Password
            });

            _clientSeller = _factory.CreateClient();
            await Utilities.Register(_clientSeller, _testUserSeller);
            _loginDataSeller = await Utilities.Login(_clientSeller, new LoginDto
            {
                Username = _testUserSeller.Username,
                Password = _testUserSeller.Password
            });
        }

        [Test]
        [Order(1)]
        public async Task TestPostTradeOffer()
        {
            var response = await Utilities.SendAuthorizedRequest(
                _clientSeller,
                _loginDataSeller,
                HttpMethod.Post,
                "/api/TradeOffers",
                _testOffer);
            response.EnsureSuccessStatusCode();
            _testOfferGetDto = await Utilities.Deserialize<TradeOfferGetDto>(response);
        }

        [Test]
        [Order(2)]
        public async Task TestGetTradeOffers()
        {
            var response = await Utilities.SendNoBodyRequest(
                _clientBuyer,
                HttpMethod.Get,
                "/api/TradeOffers");
            response.EnsureSuccessStatusCode();
            var offers = await Utilities.Deserialize<List<TradeOfferGetDto>>(response);
            var offer = (from o in offers where o == _testOfferGetDto select o).FirstOrDefault();
            Assert.IsNotNull(offer);
        }

        [Test]
        [Order(2)]
        public async Task TestGetOfferById()
        {
            var response = await Utilities.SendNoBodyRequest(
                _clientBuyer,
                HttpMethod.Get,
                "/api/TradeOffers/" + _testOfferGetDto.Id);
            response.EnsureSuccessStatusCode();
            var offer = await Utilities.Deserialize<TradeOfferGetDto>(response);
            Assert.True(offer == _testOfferGetDto);
        }

        [Test]
        [Order(3)]
        public async Task TestPatchTrade()
        {
            _testOffer.Name = "New name for trade offer";
            _testOffer.Description += " (edited)";
            _testOffer.Offer = "A soul";

            _testOfferGetDto.Name = _testOffer.Name;
            _testOfferGetDto.Description = _testOffer.Description;
            _testOfferGetDto.Offer = _testOffer.Offer;

            var response = await Utilities.SendAuthorizedRequest(
               _clientSeller,
               _loginDataSeller,
               HttpMethod.Patch,
               "/api/TradeOffers/" + _testOfferGetDto.Id,
               _testOffer);
            response.EnsureSuccessStatusCode();

            response = await Utilities.SendNoBodyRequest(
                _clientBuyer,
                HttpMethod.Get,
                "/api/TradeOffers/" + _testOfferGetDto.Id);
            response.EnsureSuccessStatusCode();

            var offer = await Utilities.Deserialize<TradeOfferGetDto>(response);
            Assert.True(offer == _testOfferGetDto);
            _testOfferGetDto = offer;
        }

        [Test]
        [Order(4)]
        public async Task TestAcceptTrade()
        {
            var tempOfferAccepted = _testOffer;

            var response = await Utilities.SendNoBodyRequest(_clientBuyer, HttpMethod.Get, "/api/StudentifyAccounts");
            response.EnsureSuccessStatusCode();
            var accounts = await Utilities.Deserialize<List<StudentifyAccountGetDto>>(response);
            tempOfferAccepted.BuyerId = (from a in accounts where a == _testUserBuyer select a.Id).FirstOrDefault();

            response = await Utilities.SendAuthorisedNoBodyRequest(
               _clientBuyer,
               _loginDataBuyer,
               HttpMethod.Patch,
               "/api/TradeOffers/" + _testOfferGetDto.Id + "/accept");
            response.EnsureSuccessStatusCode();

            response = await Utilities.SendNoBodyRequest(
                _clientBuyer,
                HttpMethod.Get,
                "/api/TradeOffers/" + _testOfferGetDto.Id);
            response.EnsureSuccessStatusCode();

            var offer = await Utilities.Deserialize<TradeOfferGetDto>(response);
            Assert.True(offer == tempOfferAccepted);
            _testOfferGetDto = offer;
        }
    }
}
