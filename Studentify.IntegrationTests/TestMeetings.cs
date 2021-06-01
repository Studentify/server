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
        private readonly RegisterDto _testAttendant = new()
        {
            Username = "Random_attendant",
            Email = "random.attendant@test.test",
            FirstName = "Random",
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
        private MeetingGetDto _testMeetingGetGto;

        [OneTimeSetUp]
        public async Task PrepareAccountAndClient()
        {
            _client = _factory.CreateClient();
            await Register(_testUser);
            await Register(_testAttendant);
            _loginData = await Login(new LoginDto
            {
                Username = _testUser.Username,
                Password = _testUser.Password
            });
        }

        public async Task Register(RegisterDto user)
        {
            using var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("api/authenticate/register", UriKind.Relative),
                Content = new StringContent(
                    JsonSerializer.Serialize(user),
                    Encoding.UTF8,
                    MediaTypeNames.Application.Json)
            };

            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }

        private async Task<LoginGetDto> Login(LoginDto user)
        {
            using var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("api/authenticate/login", UriKind.Relative),
                Content = new StringContent(
                    JsonSerializer.Serialize(user),
                    Encoding.UTF8,
                    MediaTypeNames.Application.Json)
            };

            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var body = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<LoginGetDto>(
                body, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }
       
        private async Task<HttpResponseMessage> SendAuthorizedRequest<DtoType>(HttpMethod method, string uri, DtoType dto)
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

        private async Task<HttpResponseMessage> SendAuthorisedNoBodyRequest(HttpMethod method, string uri)
        {
            using var request = new HttpRequestMessage
            {
                Method = method,
                RequestUri = new Uri(uri, UriKind.Relative)
            };
            request.Headers.Add("Authorization", "Bearer " + _loginData.Token);
            return await _client.SendAsync(request);
        }

        private async Task<MeetingGetDto> Deserialize(HttpResponseMessage response)
        {
            var body = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<MeetingGetDto>(
                body, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }

        private async Task<List<MeetingGetDto>> DeserializeList(HttpResponseMessage response)
        {
            var body = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<MeetingGetDto>>(
                body, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }

        [Test]
        [Order(1)]
        public async Task TestPostMeeting()
        {
            var response = await SendAuthorizedRequest(HttpMethod.Post, "/api/Meetings", _testMeeting);
            response.EnsureSuccessStatusCode();
            _testMeetingGetGto = await Deserialize(response);
        }

        [Test]
        [Order(2)]
        public async Task TestGetMeetings()
        {
            var response = await SendAuthorisedNoBodyRequest(HttpMethod.Get, "/api/Meetings/");
            response.EnsureSuccessStatusCode();
            var meetings = await DeserializeList(response);
            var meeting = (from m in meetings where m == _testMeetingGetGto select m).FirstOrDefault();
            Assert.IsNotNull(meeting);
        }

        [Test]
        [Order(2)]
        public async Task TestGetMeetingById()
        {
            var uri = string.Format("/api/Meetings/{0}", _testMeetingGetGto.Id);
            var response = await SendAuthorisedNoBodyRequest(HttpMethod.Get, uri);
            response.EnsureSuccessStatusCode();
            var receivedMeeting = await Deserialize(response);
            Assert.True(_testMeetingGetGto == receivedMeeting);
        }

        [Test]
        [Order(4)]
        public async Task TestPatchMeetingById()
        {
            _testMeeting.Name = "patch_" + _testMeeting.Name;
            _testMeeting.ExpiryDate = _testMeeting.ExpiryDate.AddDays(2);
            _testMeeting.Description = "Test patching event.";
            _testMeeting.Longitude = 0;
            _testMeeting.Latitude += 1.5;
            _testMeeting.Address = new()
            {
                Country = "Niemcy",
                Town = "Berlin",
                PostalCode = "00-000",
                Street = "Geschlechtsverkehrstrasse",
                HouseNumber = "18"
            };
            _testMeeting.MaxNumberOfParticipants += 1;

            var uri = string.Format("/api/Meetings/{0}", _testMeetingGetGto.Id);
            var response = await SendAuthorizedRequest(HttpMethod.Patch, uri, _testMeeting);
            response.EnsureSuccessStatusCode();

            var getUri = string.Format("/api/Meetings/{0}", _testMeetingGetGto.Id);
            var getResponse = await SendAuthorisedNoBodyRequest(HttpMethod.Get, getUri);
            response.EnsureSuccessStatusCode();
            var receivedMeeting = await Deserialize(getResponse);
            Assert.True(receivedMeeting == _testMeeting);
        }

        [Test]
        [Order(3)]
        public async Task FailToAddOwnerAsAttendant()
        {
            var uri = string.Format("/api/Meetings/attend/{0}", _testMeetingGetGto.Id);
            var response = await SendAuthorisedNoBodyRequest(HttpMethod.Patch, uri);
            try
            {
                response.EnsureSuccessStatusCode();
                Assert.Fail();
            }
            catch (HttpRequestException)
            {
                Assert.Pass();
            }
        }

        [Test]
        [Order(3)]
        public async Task TestPatchAttendToMeeting()
        {
            var ownerClient = _client;
            var ownerLoginData = _loginData;

            _client = _factory.CreateClient();
            _loginData = await Login(new LoginDto
            {
                Username = _testAttendant.Username,
                Password = _testAttendant.Password
            });

            var uri = string.Format("/api/Meetings/attend/{0}", _testMeetingGetGto.Id);
            var response = await SendAuthorisedNoBodyRequest(HttpMethod.Patch, uri);
            response.EnsureSuccessStatusCode();

            _client = ownerClient;
            _loginData = ownerLoginData;
        }

        [Test]
        [Order(4)]
        public async Task FailToAddTheSameAttendantTwice()
        {
            var ownerClient = _client;
            var ownerLoginData = _loginData;

            _client = _factory.CreateClient();
            _loginData = await Login(new LoginDto
            {
                Username = _testAttendant.Username,
                Password = _testAttendant.Password
            });

            var uri = string.Format("/api/Meetings/attend/{0}", _testMeetingGetGto.Id);
            var response = await SendAuthorisedNoBodyRequest(HttpMethod.Patch, uri);
            try
            {
                response.EnsureSuccessStatusCode();
                Assert.Fail();
            }
            catch (HttpRequestException)
            {
                Assert.Pass();
            }
            
            _client = ownerClient;
            _loginData = ownerLoginData;
        }
    }
}
