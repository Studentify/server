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
    public class InitialsController : ControllerBase
    {
        private readonly StudentifyDbContext _context;

        public InitialsController(StudentifyDbContext context)
        {
            _context = context;
        }

        // GET: api/Initials
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Initial>>> GetInitial()
        {
            return await _context.Initial.ToListAsync();
        }
        
        // [HttpGet("getSAccount/{id}")]
        // public async Task<ActionResult<StudentifyAccount>> GetSAccount(int id)
        // {
        //     var infoEvent = await _context.Infos.FindAsync(id);
        //     await _context.Entry(infoEvent).Reference(i => i.Author).LoadAsync();
        //     await _context.Entry(infoEvent.Author).Collection(a => a.Events).LoadAsync();
        //     return infoEvent.Author;
        // }

        // GET: api/Initials/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Initial>> GetInitial(int id)
        {
            var initial = await _context.Initial.FindAsync(id);

            if (initial == null)
            {
                return NotFound();
            }

            return initial;
        }

        // PUT: api/Initials/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutInitial(int id, Initial initial)
        {
            if (id != initial.Id)
            {
                return BadRequest();
            }

            _context.Entry(initial).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InitialExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Initials
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Initial>> PostInitial(Initial initial)
        {
            _context.Initial.Add(initial);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetInitial", new { id = initial.Id }, initial);
        }

        // DELETE: api/Initials/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInitial(int id)
        {
            var initial = await _context.Initial.FindAsync(id);
            if (initial == null)
            {
                return NotFound();
            }

            _context.Initial.Remove(initial);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool InitialExists(int id)
        {
            return _context.Initial.Any(e => e.Id == id);
        }
    }
}
