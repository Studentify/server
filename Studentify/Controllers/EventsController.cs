using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Studentify.Data;
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
    }
}
