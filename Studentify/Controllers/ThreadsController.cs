using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Studentify.Data;
using Studentify.Models.HttpBody;
using Studentify.Models.Messages;
using Studentify.Models.StudentifyEvents;

namespace Studentify.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ThreadsController : ControllerBase
    {
        private readonly StudentifyDbContext _context;

        public ThreadsController(StudentifyDbContext context)
        {
            _context = context;
        }

        // GET: api/Threads
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Thread>>> GetThreads()
        {
            await _context.Threads.Include(t => t.UserAccount).LoadAsync();
            return await _context.Threads.ToListAsync();
        }

        // GET: api/Threads/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Thread>> GetThread(int id)
        {
            var thread = await _context.Threads.FindAsync(id);

            if (thread == null)
            {
                return NotFound();
            }

            await _context.Threads.Include(t => t.UserAccount).LoadAsync();
            return thread;
        }

        // POST: api/Threads
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Thread>> PostThread(int eventId)
        {
            var username = User.Identity.Name;
            StudentifyAccountManager accountManager = new StudentifyAccountManager(_context);
            var account = await accountManager.FindAccountByUsername(username);

            var thread = new Thread()
            {
                EventId = eventId,
                UserAccount = account
            };

            _context.Threads.Add(thread);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetThread), new { id = thread.Id }, thread);
        }

        // GET: api/Threads/Messages
        [HttpGet("Messages")]
        public async Task<ActionResult<IEnumerable<Message>>> GetAllMessages()
        {
            await _context.Threads.Include(m => m.UserAccount).LoadAsync();
            return await _context.Messages.ToListAsync();
        }

        // GET: api/Threads/Messages/5
        [HttpGet("Messages/{threadId}")]
        public async Task<ActionResult<IEnumerable<Message>>> GetMessages(int threadId)
        {
            await _context.Threads.Include(m => m.UserAccount).LoadAsync();
            var messages = await _context.Messages.ToListAsync();
            return messages.Where(m => m.ThreadId == threadId).ToList();
        }

        // GET: api/Threads/Message/5
        [HttpGet("Message/{id}")]
        public async Task<ActionResult<Message>> GetMessage(int id)
        {
            var message = await _context.Messages.FindAsync(id);

            if (message == null)
            {
                return NotFound();
            }

            await _context.Threads.Include(m => m.UserAccount).LoadAsync();
            return message;
        }

        // POST: api/Threads/Messages
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("Messages")]
        [Authorize]
        public async Task<ActionResult<Thread>> PostMessage(MessageDto messageDto)
        {
            var username = User.Identity.Name;
            StudentifyAccountManager accountManager = new StudentifyAccountManager(_context);
            var account = await accountManager.FindAccountByUsername(username);

            await _context.Threads.Include(t => t.Messages).LoadAsync();
            var thread = await _context.Threads.FindAsync(messageDto.ThreadId);

            var message = new Message()
            {
                Author = account,
                Date = DateTime.Now,
                Content = messageDto.Content,
                IsViewed = false,
                Thread = thread
            };

            _context.Messages.Add(message);
            thread.Messages.Add(message);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMessage), new { id = message.Id }, message);
        }
    }
}
