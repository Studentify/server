using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using Studentify.Data;
using Studentify.Data.Repositories;
using Studentify.Data.Repositories.ControllerRepositories.Interfaces;
using Studentify.Models;
using Studentify.Models.HttpBody;
using Studentify.Models.StudentifyEvents;

namespace Studentify.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeetingsController : ControllerBase
    {
        private readonly IMeetingsRepository _meetingsRepository;
        private readonly IStudentifyAccountsRepository _accountsRepository;

        public MeetingsController(IMeetingsRepository meetingsRepository, IStudentifyAccountsRepository accountsRepository)
        {
            _meetingsRepository = meetingsRepository;
            _accountsRepository = accountsRepository;
        }

        // GET: api/Meetings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Meeting>>> GetMeeting()
        {
            var meetings = await _meetingsRepository.Select.All();
            return meetings.ToList();
        }

        // GET: api/Meetings/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Meeting>> GetMeeting(int id)
        {
            var meeting = await _meetingsRepository.Select.ById(id);

            if (meeting == null)
            {
                return NotFound();
            }

            return meeting;
        }

        [Authorize]
        [HttpPatch("attend/{id}")]
        public async Task<IActionResult> RegisterInterested(int id)
        {
            try
            {
                //await _context.Meetings.Include(m => m.Participants).LoadAsync();
                //var meeting = await _context.Meetings.FindAsync(id);
                var meeting = await _meetingsRepository.Select.ById(id);

                if (meeting == null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, $"There is no meeting for id = {id}");
                }
                if (meeting.Participants.Count == meeting.MaxNumberOfParticipants)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, $"Maximum number of participants has been reached");
                }

                var username = User.Identity.Name;
                var account = await _accountsRepository.SelectByUsername(username);

                if (meeting.Participants.Contains(account))
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, $"User already participates");
                }

                await _meetingsRepository.RegisterAttendance(meeting, account);
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound();
            }

            return Ok("Participation saved");
        }

        // PUT: api/Infos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPatch("{id}")]
        public async Task<IActionResult> PutMeeting(int id, MeetingDto meetingDto)
        {
            Meeting meeting;
            
            try
            {
                meeting = await _meetingsRepository.Select.ById(id);
            }
            catch (DataException)
            {
                return BadRequest();
            }

            meeting.Name = meetingDto.Name;
            meeting.ExpiryDate = meetingDto.ExpiryDate;
            meeting.MapPoint.X = meetingDto.Longitude;
            meeting.MapPoint.Y = meetingDto.Latitude;
            meeting.Address = meetingDto.Address;
            meeting.Description = meetingDto.Description;
            meeting.MaxNumberOfParticipants = meetingDto.MaxNumberOfParticipants;

            try
            {
                await _meetingsRepository.Update.One(meeting, id);
            }
            catch (DataException)
            {
                return NotFound();
            }
            
            return NoContent();
        }

        // POST: api/Meetings
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Meeting>> PostMeeting(MeetingDto meetingDto)
        {
            var username = User.Identity.Name;
            var account = await _accountsRepository.SelectByUsername(username);

            var meeting = new Meeting()
            {
                Name = meetingDto.Name,
                ExpiryDate = meetingDto.ExpiryDate,
                MapPoint = new Point(meetingDto.Longitude, meetingDto.Latitude) { SRID = 4326 },
                Address = meetingDto.Address,
                Description = meetingDto.Description,
                AuthorId = account.Id,
                CreationDate = DateTime.Now,
                MaxNumberOfParticipants = meetingDto.MaxNumberOfParticipants,
            };

            meeting.CreationDate = DateTime.Now;

            await _meetingsRepository.Insert.One(meeting);

            return CreatedAtAction(nameof(GetMeeting), new { id = meeting.Id }, meeting);
        }
    }
}