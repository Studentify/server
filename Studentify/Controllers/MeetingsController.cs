using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Studentify.Data;
using Studentify.Models.HttpBody;
using Studentify.Models.StudentifyEvents;

namespace Studentify.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeetingsController : ControllerBase
    {
        private readonly StudentifyDbContext _context;

        public MeetingsController(StudentifyDbContext context)
        {
            _context = context;
        }

        // GET: api/Meetings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Meeting>>> GetMeeting()
        {
            await _context.Meetings.Include(m => m.Participants).LoadAsync();
            return await _context.Meetings.ToListAsync();
        }

        // GET: api/Meetings/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Meeting>> GetMeeting(int id)
        {
            var studentifyMeeting = await _context.Meetings.FindAsync(id);

            if (studentifyMeeting == null)
            {
                return NotFound();
            }

            await _context.Meetings.Include(m => m.Participants).LoadAsync();
            return studentifyMeeting;
        }

        // PUT: api/Meetings/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMeeting(int id, Meeting meeting)
        {
            if (id != meeting.Id)
            {
                return BadRequest();
            }

            _context.Entry(meeting).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MeetingExists(id))
                {
                    return NotFound();
                }

                throw;
            }

            return NoContent();
        }

        [Authorize]
        [HttpPatch("attend/{id}")]
        public async Task<IActionResult> RegisterInterested(int id)
        {
            try
            {
                await _context.Meetings.Include(m => m.Participants).LoadAsync();
                var meeting = await _context.Meetings.FindAsync(id);

                if (meeting == null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, $"There is no meeting for id = {id}");
                }
                if (meeting.Participants.Count == meeting.MaxNumberOfParticipants)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, $"Maximum number of participants has been reached");
                }

                var username = User.Identity.Name;
                StudentifyAccountManager accountManager = new StudentifyAccountManager(_context);
                var account = await accountManager.FindAccountByUsername(username);

                if (meeting.Participants.Contains(account))
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, $"User already participates");
                }

                meeting.Participants.Add(account);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MeetingExists(id))
                {
                    return NotFound();
                }

                throw;
            }

            return Ok("Participation saved");
        }

        // POST: api/Meetings
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Meeting>> PostMeeting(MeetingDTO meetingDto)
        {
            var username = User.Identity.Name;
            StudentifyAccountManager accountManager = new StudentifyAccountManager(_context);
            var account = await accountManager.FindAccountByUsername(username);

            var meeting = new Meeting()
            {
                Name = meetingDto.Name,
                ExpiryDate = meetingDto.ExpiryDate,
                Location = "location",  //todo change to new Point(infoDto.Longitude, infoDto.Latitude) when it works
                Description = meetingDto.Description,
                StudentifyAccountId = account.Id,
                MaxNumberOfParticipants = meetingDto.MaxNumberOfParticipants,
            };

            meeting.CreationDate = DateTime.Now;

            _context.Meetings.Add(meeting);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMeeting), new { id = meeting.Id }, meeting);
        }

        private bool MeetingExists(int id)
        {
            return _context.Meetings.Any(e => e.Id == id);
        }
    }
}