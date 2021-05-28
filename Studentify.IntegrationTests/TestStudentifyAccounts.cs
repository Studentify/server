using NUnit.Framework;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Mime;
using System.Collections.Generic;
using System.Text;
using System;
using System.Text.Json;

using Studentify.Models.HttpBody;

namespace Studentify.IntegrationTests
{
    public class TestStudentifyAccounts
    {
        private TestAppFactory<Startup> factory;

        [SetUp]
        public async Task RegisterTestUser()
        {
            factory = new TestAppFactory<Startup>();

            var client = factory.CreateClient();
            var testUser = new RegisterDto()
            {
                Username = "Testiman",
                Email = "test@test.test",
                FirstName = "Test",
                LastName = "Testowsky",
                Password = "Test123!"
            };

            using var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("api/authenticate/register", UriKind.Relative),
                Content = new StringContent(
                    JsonSerializer.Serialize(testUser),
                    Encoding.UTF8,
                    MediaTypeNames.Application.Json)
            };

            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }

        class StudentifyAccountGetDto
        {
            public int Id { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Username { get; set; }
            public string Email { get; set; }

            private static bool CompareWithDto(StudentifyAccountGetDto a, RegisterDto dto)
            {
                if (a is null) return false;
                if (dto is null) return false;
                if (a.FirstName != dto.FirstName) return false;
                if (a.LastName != dto.LastName) return false;
                if (a.Username != dto.Username) return false;
                if (a.Email != dto.Email) return false;
                return true;
            }

            public static bool operator == (StudentifyAccountGetDto a, RegisterDto dto)
            {
                return CompareWithDto(a, dto);
            }

            public static bool operator != (StudentifyAccountGetDto a, RegisterDto dto)
            {
                return !CompareWithDto(a, dto);
            }

            public override bool Equals(object o)
            {
                RegisterDto dto = o as RegisterDto;
                if (dto is null) return false;
                return CompareWithDto(this, dto);
            }
        }

        [Test]
        public async Task CheckIfRegistered()
        {
            var expectedAccount = new RegisterDto()
            {
                Username = "Testiman",
                Email = "test@test.test",
                FirstName = "Test",
                LastName = "Testowsky",
                Password = "Test123!"
            };

            var client = factory.CreateClient();
            var response = await client.GetAsync("/api/StudentifyAccounts");
            response.EnsureSuccessStatusCode();

            var body = await response.Content.ReadAsStringAsync();
            var receivedAccount = JsonSerializer.Deserialize<List<StudentifyAccountGetDto>>(
                body, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true })[0];

            Assert.True(receivedAccount == expectedAccount);
        }
    }
}