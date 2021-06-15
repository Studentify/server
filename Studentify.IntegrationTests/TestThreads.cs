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
using Studentify.Models.Messages;

namespace Studentify.IntegrationTests
{
    class TestThreads
    {
        private readonly AppFactory<Startup> _factory = new();
        private HttpClient _clientSender;
        private HttpClient _clientRecipent;

        private readonly RegisterDto _testUserSender = new()
        {
            Username = "Testiman_thread_sender",
            Email = "test_thread_sender@test.test",
            FirstName = "Test",
            LastName = "Testowsky",
            Password = "Test123!"
        };
        private readonly RegisterDto _testUserRecipient = new()
        {
            Username = "Testiman_thread_recipient",
            Email = "test_thread_recipient@test.test",
            FirstName = "Test",
            LastName = "Testowsky",
            Password = "Test123!"
        };
        private readonly MeetingDto _testMeeting = new()
        {
            Name = "TestMeeting",
            ExpiryDate = new DateTime(2030, 1, 1, 12, 0, 0),
            Description = "This is a test meeting for all Testimankind.",
            Longitude = -50.688117,
            Latitude = 66.666667,
            Address = new Address()
            {
                Country = "Polska",
                Town = "Lipinki Łużyckie",
                PostalCode = "68-213",
                Street = "Łączna",
                HouseNumber = "43"
            },
            MaxNumberOfParticipants = 2
        };
        private MessageDto _testMessage;

        private LoginGetDto _loginDataSender;
        private LoginGetDto _loginDataRecipient;
        private ThreadGetDto _testThreadGetDto;
        private MeetingGetDto _testMeetingGetDto;
        private MessageGetDto _testMessageGetDto;

        [OneTimeSetUp]
        public async Task PrepareAccountAndClient()
        {
            //register and login both users
            _clientSender = _factory.CreateClient();
            _clientRecipent = _factory.CreateClient();
            await Utilities.Register(_clientSender, _testUserSender);
            await Utilities.Register(_clientRecipent, _testUserRecipient);
            _loginDataSender = await Utilities.Login(_clientSender, new LoginDto
            {
                Username = _testUserSender.Username,
                Password = _testUserSender.Password
            });
            _loginDataRecipient = await Utilities.Login(_clientRecipent, new LoginDto
            {
                Username = _testUserSender.Username,
                Password = _testUserSender.Password
            });
            // post meeting by recipient
            var meetingResponse = await Utilities.SendAuthorizedRequest(
                _clientRecipent,
                _loginDataRecipient,
                HttpMethod.Post,
                "/api/Meetings",
                _testMeeting);
            meetingResponse.EnsureSuccessStatusCode();
            _testMeetingGetDto = await Utilities.Deserialize<MeetingGetDto>(meetingResponse);

        }

        [Test]
        [Order(1)]
        public async Task TestPostThread()
        {
            var response = await Utilities.SendAuthorisedNoBodyRequest(
                _clientRecipent,
                _loginDataRecipient,
                HttpMethod.Post,
                "/api/Threads?eventId=" + _testMeetingGetDto.Id);
            response.EnsureSuccessStatusCode();
            _testThreadGetDto = await Utilities.Deserialize<ThreadGetDto>(response);
        }

        [Test]
        [Order(2)]
        public async Task TestGetThreads()
        {
            var response = await Utilities.SendNoBodyRequest(
                _clientRecipent,
                HttpMethod.Get,
                "/api/Threads");
            response.EnsureSuccessStatusCode();
            var threads = await Utilities.Deserialize<List<ThreadGetDto>>(response);
            var thread = (from t in threads where t == _testThreadGetDto select t).FirstOrDefault();
            Assert.IsNotNull(thread);
        }

        [Test]
        [Order(2)]
        public async Task TestGetThreadById()
        {
            var response = await Utilities.SendNoBodyRequest(
                _clientRecipent,
                HttpMethod.Get,
                "/api/Threads/" + _testThreadGetDto.Id);
            response.EnsureSuccessStatusCode();
            var thread = await Utilities.Deserialize<ThreadGetDto>(response);
            Assert.True(thread == _testThreadGetDto);
        }

        [Test]
        [Order(3)]
        public async Task TestPostMessage()
        {
            _testMessage = new MessageDto()
            {
                Content = "Szczęść Boże",
                ThreadId = _testThreadGetDto.Id
            };
            var response = await Utilities.SendAuthorizedRequest(
                _clientSender,
                _loginDataSender,
                HttpMethod.Post,
                "/api/Threads/Messages",
                _testMessage);
            response.EnsureSuccessStatusCode();
            _testMessageGetDto = await Utilities.Deserialize<MessageGetDto>(response);
            Assert.True(_testMessageGetDto == _testMessage);
        }

        [Test]
        [Order(4)]
        public async Task TestGetMessagesForThread()
        {
            var response = await Utilities.SendAuthorisedNoBodyRequest(
                _clientSender,
                _loginDataSender,
                HttpMethod.Get,
                "/api/Threads/" + _testThreadGetDto.Id + "/Messages");
            response.EnsureSuccessStatusCode();
            var messages = await Utilities.Deserialize<List<MessageGetDto>>(response);
            var message = (from m in messages where m == _testMessageGetDto select m).FirstOrDefault();
            Assert.IsNotNull(message);
        }

        [Test]
        [Order(5)]
        public async Task TestGetMessageById()
        {
            var response = await Utilities.SendAuthorisedNoBodyRequest(
                _clientSender,
                _loginDataSender,
                HttpMethod.Get,
                "/api/Threads/Messages/" + _testMessageGetDto.Id);
            response.EnsureSuccessStatusCode();
            var message = await Utilities.Deserialize<MessageGetDto>(response);
            Assert.True(message == _testMessageGetDto);
        }
    }
}
