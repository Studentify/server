using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Studentify.Data;
using Studentify.Data.Repositories;
using Studentify.Data.Repositories.ControllerRepositories.Interfaces;
using Studentify.Models.HttpBody;
using Studentify.Models.Messages;
using Studentify.Models.StudentifyEvents;

namespace Studentify.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ThreadsController : ControllerBase
    {
        private readonly IThreadsRepository _threadsRepository;
        private readonly IMessagesRepository _messagesRepository;
        private readonly IStudentifyAccountsRepository _accountsRepository;

        public ThreadsController(IThreadsRepository threadsRepository, IMessagesRepository messagesRepository, IStudentifyAccountsRepository accountsRepository)
        {
            _threadsRepository = threadsRepository;
            _accountsRepository = accountsRepository;
            _messagesRepository = messagesRepository;
        }

        // GET: api/Threads
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Thread>>> GetThreads()
        {
            var threads = await _threadsRepository.Select.All();
            return threads.ToList();
        }

        // GET: api/Threads/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Thread>> GetThread(int id)
        {
            var thread = await _threadsRepository.Select.ById(id);

            if (thread == null)
            {
                return NotFound();
            }

            return thread;
        }

        // POST: api/Threads
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Thread>> PostThread(int eventId)
        {
            var username = User.Identity.Name;
            var account = await _accountsRepository.SelectByUsername(username);

            var thread = new Thread()
            {
                EventId = eventId,
                UserAccount = account
            };

            await _threadsRepository.Insert.One(thread);

            return CreatedAtAction(nameof(GetThread), new { id = thread.Id }, thread);
        }

        // GET: api/Threads/Messages
        [HttpGet("Messages")]
        public async Task<ActionResult<IEnumerable<Message>>> GetAllMessages()
        {
            var messages = await _messagesRepository.Select.All();
            return messages.ToList();
        }

        // GET: api/Threads/Messages/5
        [HttpGet("Messages/{threadId}")]
        public async Task<ActionResult<IEnumerable<Message>>> GetMessages(int threadId)
        {
            var messages = await _messagesRepository.SelectAllFromThread(threadId);
            return messages.ToList();
        }

        // GET: api/Threads/Message/5
        [HttpGet("Message/{id}")]
        public async Task<ActionResult<Message>> GetMessage(int id)
        {
            var message = await _messagesRepository.Select.ById(id);

            if (message == null)
            {
                return NotFound();
            }

            return message;
        }

        // POST: api/Threads/Messages
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("Messages")]
        [Authorize]
        public async Task<ActionResult<Thread>> PostMessage(MessageDto messageDto)
        {
            var username = User.Identity.Name;
            var account = await _accountsRepository.SelectByUsername(username);

            //await _context.Threads.Include(t => t.Messages).LoadAsync();
            //var thread = await _context.Threads.FindAsync(messageDto.ThreadId);
            var thread = await _threadsRepository.Select.ById(messageDto.ThreadId);

            var message = new Message()
            {
                Author = account,
                Date = DateTime.Now,
                Content = messageDto.Content,
                IsViewed = false,
                Thread = thread
            };

            await _messagesRepository.PostNewMessage(thread, message);

            return CreatedAtAction(nameof(GetMessage), new { id = message.Id }, message);
        }
    }
}
