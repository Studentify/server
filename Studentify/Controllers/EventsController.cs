using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Studentify.Data;
using Studentify.Data.Repositories;
using Studentify.Models.StudentifyEvents;

namespace Studentify.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    // [Authorize]
    public class EventsController : ControllerBase
    {
        private readonly IStudentifyEventsRepository _repository;

        public EventsController(IStudentifyEventsRepository repository)
        {
            _repository = repository;
        }

        // GET: api/Events
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StudentifyEvent>>> GetEvents()
        {
            var studentifyEvents = await _repository.Select.All();
            return studentifyEvents.ToList();
        }

        // GET: api/Events/5
        [HttpGet("{id}")]
        public async Task<ActionResult<StudentifyEvent>> GetEvent(int id)
        {
            var studentifyEvent = await _repository.Select.ById(id);

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
            try
            {
                await _repository.Delete.ById(id);
            }
            catch (DataException e)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
