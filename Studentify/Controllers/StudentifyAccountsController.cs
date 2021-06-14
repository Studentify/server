using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Studentify.Data.Repositories;
using Studentify.Data.Repositories.ControllerRepositories.Interfaces;
using Studentify.Models;
using Studentify.Models.DTO;
using Studentify.Models.HttpBody;

namespace Studentify.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentifyAccountsController : ControllerBase
    {
        private readonly IStudentifyAccountsRepository _accountsRepository;
        private readonly ISkillsRepository _skillsRepository;

        public StudentifyAccountsController(IStudentifyAccountsRepository accountsRepository, ISkillsRepository skillsRepository)
        {
            _accountsRepository = accountsRepository;
            _skillsRepository = skillsRepository;
        }

        // GET: api/Initials
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StudentifyAccount>>> GetStudentifyAccounts()
        {
            var accounts = await _accountsRepository.Select.All();
            return accounts.ToList();
        }

        // GET: api/Initials/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<StudentifyAccount>> GetStudentifyAccount(int id)
        {
            var account = await _accountsRepository.Select.ById(id);

            if (account == null)
            {
                return NotFound();
            }

            return account;
        }

        // PATCH: api/StudentifyAccounts
        [Authorize]
        [HttpPatch]
        public async Task<IActionResult> PutStudentifyAccount(StudentifyAccountDto accountDto)
        {
            var username = User.Identity.Name;
            StudentifyAccount account;

            try
            {
                account = await _accountsRepository.SelectByUsername(username);
            }
            catch (DataException)
            {
                return BadRequest();
            }

            account.User.FirstName = accountDto.FirstName;
            account.User.LastName = accountDto.LastName;

            try
            {
                await _accountsRepository.Update.One(account, account.Id);
            }
            catch (DataException)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpGet("Skills")]
        public async Task<ActionResult<IEnumerable<Skill>>> GetAccountSkills(int id)
        {
            var skills = await _accountsRepository.GetSkills(id);
            return skills.ToList();
        }

        [HttpPost("Skills")]
        public async Task PostAccountSkill(SkillDto skillDto)
        {
            var owner = await _accountsRepository.Select.ById(skillDto.OwnerId);
            var skill = new Skill()
            {
                Name = skillDto.Name,
                Owner = owner,
                Rate = skillDto.Rate
            };

            await _skillsRepository.Insert.One(skill);
            await _accountsRepository.SaveSkill(skillDto.OwnerId, skill);
        }
    }
}
