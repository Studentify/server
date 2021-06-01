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
    class TestMeetings
    {
        private readonly AppFactory<Startup> _factory = new();
        private HttpClient _client;
        private LoginGetDto _loginData;
        private readonly RegisterDto _testUser = new()
        {
            Username = "Testiman_meeting",
            Email = "test_meeting@test.test",
            FirstName = "Test",
            LastName = "Testowsky",
            Password = "Test123!"
        };
        private MeetingDto _testMeeting = new()
        {
            Name = "TestMeeting",
            ExpiryDate = new DateTime(2030, 1, 1, 12, 0, 0),
            Description = "This is a test meeting for all Testimankind.",
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

        [OneTimeSetUp]
        public async Task PrepareAccountAndClient()
        {
            _client = _factory.CreateClient();
            await Register();
            _loginData = await Login();
        }

        public async Task Register()
        {
            using var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("api/authenticate/register", UriKind.Relative),
                Content = new StringContent(
                    JsonSerializer.Serialize(_testUser),
                    Encoding.UTF8,
                    MediaTypeNames.Application.Json)
            };

            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }

        private async Task<LoginGetDto> Login()
        {
            using var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("api/authenticate/login", UriKind.Relative),
                Content = new StringContent(
                    JsonSerializer.Serialize(
                    new LoginDto
                    {
                        Username = _testUser.Username,
                        Password = _testUser.Password
                    }),
                    Encoding.UTF8,
                    MediaTypeNames.Application.Json)
            };

            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var body = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<LoginGetDto>(
                body, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }

        private async Task<HttpResponseMessage> SendAuthorizedRequest(HttpMethod method, String uri, MeetingDto dto)
        {
            using var request = new HttpRequestMessage
            {
                Method = method,
                RequestUri = new Uri(uri, UriKind.Relative),
                Content = new StringContent(JsonSerializer.Serialize(dto),
                                            Encoding.UTF8,
                                            MediaTypeNames.Application.Json)
            };
            request.Headers.Add("Authorization", "Bearer " + _loginData.Token);
            return await _client.SendAsync(request);
        }

        [Test]
        public async Task TestGetMeetings()
        {
            Assert.Pass();
        }

        [Test]
        public async Task TestPostMeeting()
        {
            var response = await SendAuthorizedRequest(HttpMethod.Post, "/api/Meetings", _testMeeting);
            response.EnsureSuccessStatusCode();
        }

        [Test]
        public async Task TestGetMeetingById()
        {
            Assert.Pass();
        }

        [Test]
        public async Task TestPatchMeetingById()
        {
            Assert.Pass();
        }

        [Test]
        public async Task TestPatchAttend()
        {
            Assert.Pass();
        }
    }
}
