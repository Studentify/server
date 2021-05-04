using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Studentify.Data;
using Studentify.Models;

namespace Studentify.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentifyAccountsController : ControllerBase
    {
        private readonly StudentifyDbContext _context;

        public StudentifyAccountsController(StudentifyDbContext context)
        {
            _context = context;
        }

        // GET: api/Initials
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StudentifyAccount>>> GetStudentifyAccounts()
        {
            var studentifyAccounts = await _context.StudentifyAccounts.ToListAsync();
            // foreach (var studentifyAccount in studentifyAccounts)
            // {
            //     await _context.Entry(studentifyAccount).Collection(s => s.Events).LoadAsync();
            // }

            return studentifyAccounts;
        }
        
        // GET: api/Initials/5
        [HttpGet("{id}")]
        public async Task<ActionResult<StudentifyAccount>> GetStudentifyAccount(int id)
        {
            var initial = await _context.StudentifyAccounts.FindAsync(id);

            if (initial == null)
            {
                return NotFound();
            }

            return initial;
        }

        // // PUT: api/Initials/5
        // // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        // [HttpPut("{id}")]
        // public async Task<IActionResult> PutInitial(int id, Initial initial)
        // {
        //     if (id != initial.Id)
        //     {
        //         return BadRequest();
        //     }
        //
        //     _context.Entry(initial).State = EntityState.Modified;
        //
        //     try
        //     {
        //         await _context.SaveChangesAsync();
        //     }
        //     catch (DbUpdateConcurrencyException)
        //     {
        //         if (!InitialExists(id))
        //         {
        //             return NotFound();
        //         }
        //         else
        //         {
        //             throw;
        //         }
        //     }
        //
        //     return NoContent();
        // }
        //
        // private bool InitialExists(int id)
        // {
        //     return _context.Initial.Any(e => e.Id == id);
        // }
    }
}
