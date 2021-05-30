using NUnit.Framework;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System;
using System.Text.Json;

using Studentify.Models.HttpBody;

namespace Studentify.IntegrationTests
{
    class TestAuthenticate
    {
        private readonly AppFactory<Startup> _factory = new();
        private HttpClient _client;
        private readonly RegisterDto _testUser = new()
        {
            Username = "Testiman_auth",
            Email = "test_auth@test.test",
            FirstName = "Test",
            LastName = "Testowsky",
            Password = "Test123!"
        };

        [SetUp]
        public void CreateNewClient()
        {
            _client = _factory.CreateClient();
        }

        [Test]
        public async Task TestRegisterAndLogin()
        {
            using var registerRequest = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("api/authenticate/register", UriKind.Relative),
                Content = new StringContent(
                    JsonSerializer.Serialize(_testUser),
                    Encoding.UTF8,
                    MediaTypeNames.Application.Json)
            };

            var registerResponse = await _client.SendAsync(registerRequest);
            registerResponse.EnsureSuccessStatusCode();

            using var loginRequest = new HttpRequestMessage
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

            var loginResponse = await _client.SendAsync(loginRequest);
            loginResponse.EnsureSuccessStatusCode();
        }
    }
}
