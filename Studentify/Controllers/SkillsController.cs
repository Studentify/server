using System;
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
using Studentify.Models.Messages;
using Studentify.Models.StudentifyEvents;

namespace Studentify.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SkillsController : ControllerBase
    {
        private readonly ISkillsRepository _skillsRepository;
        private readonly IStudentifyAccountsRepository _accountsRepository;

        public SkillsController(ISkillsRepository skillsRepository, IStudentifyAccountsRepository accountsRepository)
        {
            _skillsRepository = skillsRepository;
            _accountsRepository = accountsRepository;
        }

        // GET: api/Skills
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Skill>>> GetSkills()
        {
            var skills = await _skillsRepository.Select.All();
            return skills.ToList();
        }

        // GET: api/Skills/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Skill>> GetSkill(int id)
        {
            var skill = await _skillsRepository.Select.ById(id);

            if (skill == null)
            {
                return NotFound();
            }

            return skill;
        }

        // POST: api/Skills
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Skill>> PostSkill(SkillDto skillDto)
        {
            var account = await _accountsRepository.Select.ById(skillDto.OwnerId);

            var skill = new Skill()
            {
                Name = skillDto.Name,
                Owner = account,
                Rate = skillDto.Rate
            };

            await _skillsRepository.Insert.One(skill);
            return CreatedAtAction(nameof(GetSkill), new { id = skill.Id }, skill);
        }

        [Authorize]
        [HttpPatch("{id}")]
        public async Task UpdateSkillRate([FromRoute] int id, [FromBody] int rate)
        {
            await _skillsRepository.UpdateSkillRate(id, rate);
        }
    }
}
