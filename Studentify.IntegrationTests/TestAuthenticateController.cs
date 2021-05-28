using System;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Studentify.Models.Authentication;
using System.Net.Mime;
using System.Text;
using System.Collections.Generic;

namespace Studentify.IntegrationTests
{
    public class TestAuthenticateController
        : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;

        public TestAuthenticateController(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task TestRegister()
        {
            // Arrange
            var client = _factory.CreateClient();
            var newUser = new Dictionary<string, object>();
            newUser.Add("email", "wda@clb.ble");
            newUser.Add("firstName", "Name");
            newUser.Add("lastName", "Vorname");
            newUser.Add("username", "Marik1234");
            newUser.Add("password", "Silnia123!");

            using var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("api/authenticate/register", UriKind.Relative),
                Content = new StringContent(
                    Newtonsoft.Json.JsonConvert.SerializeObject(newUser), 
                    Encoding.UTF8, 
                    MediaTypeNames.Application.Json
)
            };

            // Act
            var response = await client.SendAsync(request);
            Console.WriteLine(response.RequestMessage);
            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            //Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("application/json; charset=utf-8",
                response.Content.Headers.ContentType.ToString());
        }
    }
}
