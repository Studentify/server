using System.Threading.Tasks;
using System.Net.Http;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Studentify.Models.HttpBody;
using Studentify.IntegrationTests.GetDto;
using Studentify.Models.DTO;

namespace Studentify.IntegrationTests
{
    class TestSkills
    {
        private readonly AppFactory<Startup> _factory = new();

        private HttpClient _clientSkilled;
        private LoginGetDto _loginDataSkilled;
        private readonly RegisterDto _testUserSkilled = new()
        {
            Username = "Testiman_skills_skilled",
            Email = "test_skills_skilled@test.test",
            FirstName = "Test",
            LastName = "Testowsky",
            Password = "Test123!"
        };
        private int _testUserSkilledId;

        private HttpClient _clientCritic;
        private LoginGetDto _loginDataCritic;
        private readonly RegisterDto _testUserCritic = new()
        {
            Username = "Testiman_skills_critic",
            Email = "test_skills_critic@test.test",
            FirstName = "Test",
            LastName = "Testowsky",
            Password = "Test123!"
        };

        private SkillDto _testSkill;
        private SkillGetDto _testSkillGetDto;

        [OneTimeSetUp]
        public async Task PrepareAccountsAndClients()
        {
            _clientSkilled = _factory.CreateClient();
            await Utilities.Register(_clientSkilled, _testUserSkilled);
            _loginDataSkilled = await Utilities.Login(_clientSkilled, new LoginDto
            {
                Username = _testUserSkilled.Username,
                Password = _testUserSkilled.Password
            });

            _clientCritic = _factory.CreateClient();
            await Utilities.Register(_clientCritic, _testUserCritic);
            _loginDataCritic = await Utilities.Login(_clientCritic, new LoginDto
            {
                Username = _testUserCritic.Username,
                Password = _testUserCritic.Password
            });

            var response = await Utilities.SendNoBodyRequest(_clientSkilled, HttpMethod.Get, "/api/StudentifyAccounts");
            response.EnsureSuccessStatusCode();
            var accounts = await Utilities.Deserialize<List<StudentifyAccountGetDto>>(response);
            _testUserSkilledId = (from a in accounts where a == _testUserSkilled select a.Id).FirstOrDefault();

            _testSkill = new()
            {
                Name = "Nie umieją testować",
                Rate = 2,
                OwnerId = _testUserSkilledId
            };
        }

        [Test]
        [Order(1)]
        public async Task TestPostSkill()
        {
            var response = await Utilities.SendAuthorizedRequest(
                _clientCritic,
                _loginDataCritic,
                HttpMethod.Post,
                "/api/Skills",
                _testSkill);
            response.EnsureSuccessStatusCode();
            _testSkillGetDto = await Utilities.Deserialize<SkillGetDto>(response);
        }

        [Test]
        [Order(1)]
        public async Task FailToAddSkillToMyself()
        {
            var response = await Utilities.SendAuthorizedRequest(
                _clientSkilled,
                _loginDataSkilled,
                HttpMethod.Post,
                "/api/Skills",
                _testSkill);
            if (response.IsSuccessStatusCode) Assert.Fail();
            else Assert.Pass();
        }

        [Test]
        [Order(2)]
        public async Task TestGetSkills()
        {
            var response = await Utilities.SendNoBodyRequest(
                _clientCritic,
                HttpMethod.Get,
                "/api/Skills");
            response.EnsureSuccessStatusCode();
            var skills = await Utilities.Deserialize<List<SkillGetDto>>(response);
            var skill = (from s in skills where s == _testSkillGetDto select s).FirstOrDefault();
            Assert.IsNotNull(skill);
        }

        [Test]
        [Order(2)]
        public async Task TestGetSkillById()
        {
            var response = await Utilities.SendNoBodyRequest(
                _clientCritic,
                HttpMethod.Get,
                "/api/Skills/" + _testSkillGetDto.Id);
            response.EnsureSuccessStatusCode();
            var skill = await Utilities.Deserialize<SkillGetDto>(response);
            Assert.True(skill == _testSkillGetDto);
        }
    }
}
