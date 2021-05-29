using NUnit.Framework;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Mime;
using System.Collections.Generic;
using System.Text;
using System;
using System.Text.Json;
using System.Linq;

using Studentify.Models.HttpBody;
using Studentify.IntegrationTests.GetDto;

namespace Studentify.IntegrationTests
{
    public class TestStudentifyAccounts
    {
        private readonly AppFactory<Startup> _factory = new();
        private HttpClient _client;
        private readonly RegisterDto _testUser = new()
        {
            Username = "Testiman_accounts",
            Email = "test_accounts@test.test",
            FirstName = "Test",
            LastName = "Testowsky",
            Password = "Test123!"
        };
        private LoginGetDto _loginData;

        private async Task<List<StudentifyAccountGetDto>> GetAllStudentifyAccounts()
        {
            var response = await _client.GetAsync("/api/StudentifyAccounts");
            response.EnsureSuccessStatusCode();

            var body = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<StudentifyAccountGetDto>>(
                body, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
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

        [OneTimeSetUp]
        public async Task Register()
        {
            var client = _factory.CreateClient();

            using var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("api/authenticate/register", UriKind.Relative),
                Content = new StringContent(
                    JsonSerializer.Serialize(_testUser),
                    Encoding.UTF8,
                    MediaTypeNames.Application.Json)
            };

            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }

        [SetUp]
        public async Task CreateNewClient()
        {
            _client = _factory.CreateClient();
            _loginData = await Login();
        }

        [Test]
        public async Task TestGetStudentifyAccounts()
        {
            var accounts = await GetAllStudentifyAccounts();
            var testUserAccount = (from a in accounts where a == _testUser select a).FirstOrDefault();
            //var receivedAccount = accounts.Where(a => a == _testUser).FirstOrDefault();

            Assert.False(testUserAccount is null);
        }

        [Test]
        public async Task TestPatchStudentifyAccounts()
        {
            var patchedUser = new RegisterDto()
            {
                Username = _testUser.Username,
                Password = _testUser.Password,
                Email = "patch" + _testUser.Email,
                FirstName = "patch" + _testUser.FirstName,
                LastName = "patch" + _testUser.LastName
            };

            var patchedUserDto = new StudentifyAccountDto()
            {
                Email = patchedUser.Email,
                FirstName = patchedUser.FirstName,
                LastName = patchedUser.LastName
            };

            using var request = new HttpRequestMessage
            {
                Method = HttpMethod.Patch,
                RequestUri = new Uri("/api/StudentifyAccounts", UriKind.Relative), 
                Content = new StringContent(JsonSerializer.Serialize(patchedUserDto),
                    Encoding.UTF8,
                    MediaTypeNames.Application.Json)
            };

            request.Headers.Add("Authorization", "Bearer " + _loginData.Token);

            var reponse = await _client.SendAsync(request);
            reponse.EnsureSuccessStatusCode();

            var accounts = await GetAllStudentifyAccounts();
            var testUserAccount = (from a in accounts where a == patchedUser select a).FirstOrDefault();

            Assert.False(testUserAccount is null);
        }
    }
}