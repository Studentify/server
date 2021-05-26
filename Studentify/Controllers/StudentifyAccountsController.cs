using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Studentify.Data;
using Studentify.Data.Repositories;
using Studentify.Models;
using Studentify.Models.HttpBody;

namespace Studentify.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentifyAccountsController : ControllerBase
    {
        private readonly IStudentifyAccountsRepository _accountsRepository;

        public StudentifyAccountsController(IStudentifyAccountsRepository accountsRepository)
        {
            _accountsRepository = accountsRepository;
        }

        // GET: api/Initials
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StudentifyAccount>>> GetStudentifyAccounts()
        {
            var accounts = await _accountsRepository.Select.All();
            return accounts.ToList();
        }
        
        // GET: api/Initials/5
        [HttpGet("{id}")]
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

            account.User.UserName = accountDto.UserName;    //todo make changing username work
            account.User.FirstName = accountDto.FirstName;
            account.User.LastName = accountDto.LastName;
            account.User.Email = accountDto.Email;
            
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
    }
}
