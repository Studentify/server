using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
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
    [Authorize]
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
            return new(await _repository.GetAll());
        }

        // GET: api/Events/5
        [HttpGet("{id}")]
        public async Task<ActionResult<StudentifyEvent>> GetEvent(int id)
        {
            var studentifyEvent = await _repository.FindById(id);

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
                await _repository.RemoveById(id);
            }
            catch (DataException e)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
