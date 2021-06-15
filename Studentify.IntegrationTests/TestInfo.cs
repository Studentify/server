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
using Studentify.Models.StudentifyEvents;
using Studentify.IntegrationTests.GetDto;

namespace Studentify.IntegrationTests
{
    class TestInfo
    {
        private readonly AppFactory<Startup> _factory = new();
        private HttpClient _client;
        private LoginGetDto _loginData;
        private readonly RegisterDto _testUser = new()
        {
            Username = "Testiman_info",
            Email = "test_info@test.test",
            FirstName = "Test",
            LastName = "Testowsky",
            Password = "Test123!"
        };
        private InfoDto _testInfo = new()
        {
            Name = "TestMeeting",
            ExpiryDate = new DateTime(2030, 1, 1, 12, 0, 0),
            Description = "Alaaaarm!",
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
            Category = InfoCategory.Alert
        };
        private InfoGetDto _testInfoGetDto;

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
        }

        [Test]
        [Order(1)]
        public async Task TestPostInfo()
        {
            var response = await Utilities.SendAuthorizedRequest(_client, _loginData, HttpMethod.Post, "/api/Info", _testInfo);
            response.EnsureSuccessStatusCode();
            _testInfoGetDto = await Utilities.Deserialize<InfoGetDto>(response);
            Assert.IsNotNull(_testInfoGetDto);
        }

        [Test]
        [Order(2)]
        public async Task TestGetInfo()
        {
            var response = await Utilities.SendAuthorisedNoBodyRequest(_client, _loginData, HttpMethod.Get, "/api/Info/");
            response.EnsureSuccessStatusCode();
            var infos = await Utilities.Deserialize<List<InfoGetDto>>(response);
            var info = (from i in infos where i == _testInfoGetDto select i).FirstOrDefault();
            Assert.IsNotNull(info);
        }

        [Test]
        [Order(2)]
        public async Task TestGetInfoById()
        {
            var uri = string.Format("/api/Info/{0}", _testInfoGetDto.Id);
            var response = await Utilities.SendAuthorisedNoBodyRequest(_client, _loginData, HttpMethod.Get, uri);
            response.EnsureSuccessStatusCode();
            var receivedInfo = await Utilities.Deserialize<InfoGetDto>(response);
            Assert.True(_testInfoGetDto == receivedInfo);
        }

        [Test]
        [Order(3)]
        public async Task TestPatchInfoById()
        {
            _testInfo.Name = "patch_" + _testInfo.Name;
            _testInfo.ExpiryDate = _testInfo.ExpiryDate.AddDays(2);
            _testInfo.Description = "Juz nie alarm.";
            _testInfo.Longitude -= 1;
            _testInfo.Latitude += 1.5;
            _testInfo.Address = new()
            {
                Country = "Niemcy",
                Town = "Berlin",
                PostalCode = "00-000",
                Street = "Geschlechtsverkehrstrasse",
                HouseNumber = "18"
            };
            _testInfo.Category = InfoCategory.Notice;

            var uri = string.Format("/api/Info/{0}", _testInfoGetDto.Id);
            var response = await Utilities.SendAuthorizedRequest(_client, _loginData, HttpMethod.Patch, uri, _testInfo);
            response.EnsureSuccessStatusCode();

            var getUri = string.Format("/api/Info/{0}", _testInfoGetDto.Id);
            var getResponse = await Utilities.SendAuthorisedNoBodyRequest(_client, _loginData, HttpMethod.Get, getUri);
            response.EnsureSuccessStatusCode();
            var receivedInfo = await Utilities.Deserialize<InfoGetDto>(getResponse);
            Assert.True(receivedInfo == _testInfo);
        }
    }
}
