using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Studentify.Data;
using Studentify.Data.Repositories;
using Studentify.Data.Repositories.ControllerRepositories.Interfaces;
using Studentify.Models;
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
        private readonly IStudentifyEventsRepository _eventsRepository;

        public ThreadsController(IThreadsRepository threadsRepository, IMessagesRepository messagesRepository, IStudentifyAccountsRepository accountsRepository, IStudentifyEventsRepository studentifyEventsRepository)
        {
            _threadsRepository = threadsRepository;
            _accountsRepository = accountsRepository;
            _messagesRepository = messagesRepository;
            _eventsRepository = studentifyEventsRepository;
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
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Thread>> PostThread(int eventId)
        {
            var username = User.Identity.Name;
            StudentifyAccount account;
            try
            {
                account = await _accountsRepository.SelectByUsername(username);
            }
            catch (DataException)
            {
                return BadRequest($"Cannot get id of logged account.");
            }

            StudentifyEvent referencedEvent;
            try
            {
                referencedEvent = await _eventsRepository.Select.ById(eventId);
            }
            catch (DataException)
            {
                return NotFound($"Event of id {eventId} not found.");
            }

            var thread = new Thread()
            {
                ReferencedEvent = referencedEvent,
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

        // GET: api/Threads/5/Messages
        [HttpGet("{threadId:int}/Messages")]
        public async Task<ActionResult<IEnumerable<Message>>> GetMessagesFromThread(int threadId)
        {
            var messages = await _messagesRepository.SelectAllFromThread(threadId);
            return messages.ToList();
        }

        // GET: api/Threads/Messages/5
        [HttpGet("Messages/{id:int}")]
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
        [Authorize]
        [HttpPost("Messages")]
        public async Task<ActionResult<Thread>> PostMessage(MessageDto messageDto)
        {
            var username = User.Identity.Name;
            var account = await _accountsRepository.SelectByUsername(username);

            try
            {
                var thread = await _threadsRepository.Select.ById(messageDto.ThreadId);
                var message = new Message()
                {
                    Author = account,
                    Date = DateTime.Now,
                    Content = messageDto.Content,
                    IsViewed = false,
                    Thread = thread
                };

                await _messagesRepository.InsertMessageToThread(message, thread);
                
                return CreatedAtAction(nameof(GetMessage), new { id = message.Id }, message);
            }
            catch (DataException)
            {
                return NotFound($"Thread of id {messageDto.ThreadId} not found.");
            }
        }
    }
}
