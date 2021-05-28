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
        private readonly TestAppFactory<Startup> _factory = new();
        private HttpClient _client;
        private readonly RegisterDto _testUser = new()
        {
            Username = "Testiman",
            Email = "test@test.test",
            FirstName = "Test",
            LastName = "Testowsky",
            Password = "Test123!"
        };

        [OneTimeSetUp]
        public async Task RegisterTestUser()
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
        public void CreateNewClient()
        {
            _client = _factory.CreateClient();
        }

        [Test]
        public async Task CheckIfRegistered()
        {
            var response = await _client.GetAsync("/api/StudentifyAccounts");
            response.EnsureSuccessStatusCode();

            var body = await response.Content.ReadAsStringAsync();
            var accounts = JsonSerializer.Deserialize<List<StudentifyAccountGetDto>>(body, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            
            var receivedAccount = (from a in accounts where a == _testUser select a).FirstOrDefault();
            //var receivedAccount = accounts.Where(a => a == _testUser).FirstOrDefault();

            Assert.False(receivedAccount is null);
        }
    }
}