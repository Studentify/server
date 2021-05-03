using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Studentify.Data;
using Studentify.Models;
using Studentify.Models.HttpBody;
using Studentify.Models.StudentifyEvents;

namespace Studentify.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly StudentifyDbContext _context;

        public EventsController(StudentifyDbContext context)
        {
            _context = context;
        }

        // GET: api/Events
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StudentifyEvent>>> GetEvents()
        {
            return await _context.Events.ToListAsync();
        }

        // GET: api/Events/5
        [HttpGet("{id}")]
        public async Task<ActionResult<StudentifyEvent>> GetEvent(int id)
        {
            var studentifyEvent = await _context.Events.FindAsync(id);

            if (studentifyEvent == null)
            {
                return NotFound();
            }

            return studentifyEvent;
        }

        // PUT: api/Events/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEvent(int id, StudentifyEvent studentifyEvent)
        {
            if (id != studentifyEvent.Id)
            {
                return BadRequest();
            }

            _context.Entry(studentifyEvent).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EventExists(id))
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

        // POST: api/Events
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<StudentifyEvent>> PostEvent(EventDTO eventDto)
        {
            var studentifyEvent = new StudentifyEvent()
            {
                Name = eventDto.Name,
                ExpiryDate = eventDto.ExpiryDate,
                Location = eventDto.Location,
                Description = eventDto.Description,
                StudentifyAccountId = eventDto.StudentifyAccountId
            };
            studentifyEvent.Author = await _context.StudentifyAccounts.FindAsync(studentifyEvent.StudentifyAccountId);
            studentifyEvent.CreationDate = DateTime.Now;
            
            _context.Events.Add(studentifyEvent);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEvent", new { id = studentifyEvent.Id }, studentifyEvent);
        }

        // DELETE: api/Events/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var studentifyEvent = await _context.Events.FindAsync(id);
            if (studentifyEvent == null)
            {
                return NotFound();
            }

            _context.Events.Remove(studentifyEvent);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EventExists(int id)
        {
            return _context.Events.Any(e => e.Id == id);
        }
    }
}
