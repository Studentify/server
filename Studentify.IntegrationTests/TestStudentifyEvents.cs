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
    class TestStudentifyEvents
    {
        private readonly AppFactory<Startup> _factory = new();
        private HttpClient _client;
        private readonly RegisterDto _testUser = new()
        {
            Username = "Testiman_events",
            Email = "test_events@test.test",
            FirstName = "Test",
            LastName = "Testowsky",
            Password = "Test123!"
        };
        private LoginGetDto _loginData;
        private MeetingDto _testEvent = new()
        {
            Name = "TestMeeting",
            ExpiryDate = new DateTime(2050, 1, 1, 21, 15, 0),
            Description = "This is a generic test event.",
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
            MaxNumberOfParticipants = 1
        };
        private StudentifyEventGetDto _testEventGetDto;

        private async Task PostEvent()
        {
            var response = await Utilities.SendAuthorizedRequest(_client, _loginData, HttpMethod.Post, "/api/Meetings", _testEvent);
            response.EnsureSuccessStatusCode();
            _testEventGetDto = await Utilities.Deserialize<StudentifyEventGetDto>(response);
        }

        [OneTimeSetUp]
        public async Task PrepareAccountAndClient()
        {
            _client = _factory.CreateClient();
            await Utilities.Register(_client, _testUser);
            _loginData = await Utilities.Login(_client, new LoginDto
            {
                Username = _testUser.Username,
                Password = _testUser.Password
            });
            await PostEvent();
        }

        [Test]
        [Order(1)]
        public async Task TestGetEvents()
        {
            var response = await Utilities.SendNoBodyRequest(
                _client,
                HttpMethod.Get,
                "/api/StudentifyEvents");
            response.EnsureSuccessStatusCode();
            var events = await Utilities.Deserialize<List<StudentifyEventGetDto>>(response);
            var receivedEvent = (from m in events where m == _testEventGetDto select m).FirstOrDefault();
            Assert.IsNotNull(receivedEvent);
        }

        [Test]
        [Order(1)]
        public async Task TestGetEventById()
        {
            var response = await Utilities.SendNoBodyRequest(
                _client,
                HttpMethod.Get,
                "/api/StudentifyEvents/" + _testEventGetDto.Id);
            response.EnsureSuccessStatusCode();
            var receivedEvent = await Utilities.Deserialize<StudentifyEventGetDto>(response);
            Assert.True(receivedEvent == _testEventGetDto);
        }

        [Test]
        [Order(2)]
        public async Task TestDeleteEvent()
        {
            var responseDeletion = await Utilities.SendAuthorisedNoBodyRequest(
                _client,
                _loginData,
                HttpMethod.Delete,
                "/api/StudentifyEvents/" + _testEventGetDto.Id);
            responseDeletion.EnsureSuccessStatusCode();

            var response = await Utilities.SendNoBodyRequest(
                _client,
                HttpMethod.Get,
                "/api/StudentifyEvents");
            response.EnsureSuccessStatusCode();
            var events = await Utilities.Deserialize<List<StudentifyEventGetDto>>(response);
            var receivedEvent = (from m in events where m == _testEventGetDto select m).FirstOrDefault();
            Assert.IsNull(receivedEvent);
        }
    }
}
