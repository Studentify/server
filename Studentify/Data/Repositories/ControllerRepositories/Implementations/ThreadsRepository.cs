using Microsoft.EntityFrameworkCore;
using Studentify.Data.Repositories.ControllerRepositories.Interfaces;
using Studentify.Models;
using Studentify.Models.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Studentify.Data.Repositories.ControllerRepositories.Implementations
{
    public class ThreadsRepository : RepositoryBase, IThreadsRepository
    {
        public IInsertRepository<Thread> Insert { get; }
        public IUpdateRepository<Thread> Update { get; }
        public ISelectRepository<Thread> Select { get; set; }
        public ISelectRepository<Message> SelectMessages { get; }
        public IInsertRepository<Message> InsertMessages { get; }
        public IUpdateRepository<Message> UpdateMessages { get; }
        public ThreadsRepository(StudentifyDbContext context,
                               ISelectRepository<Thread> selectRepository,
                               IInsertRepository<Thread> insertRepository,
                               IUpdateRepository<Thread> updateRepository,
                               ISelectRepository<Message> selectMessagesRepository,
                               IInsertRepository<Message> insertMessagesRepository,
                               IUpdateRepository<Message> updateMessagesRepository
                               ) : base(context)
        {
            Insert = insertRepository;
            Update = updateRepository;
            Select = selectRepository;

            Select.FillWithReferences += async entities =>
            {
                await Context.Entry(entities).Reference(t => t.UserAccount).LoadAsync();
                var users = await Context.Set<StudentifyAccount>().ToListAsync();
                foreach (var user in users)
                {
                    await Context.Entry(user).Reference(i => i.User).LoadAsync();
                }
            };

            InsertMessages = insertMessagesRepository;
            UpdateMessages = updateMessagesRepository;
            SelectMessages = selectMessagesRepository;

            SelectMessages.FillWithReferences += async entities =>
            {
                var threads = await Context.Set<Thread>().ToListAsync();
                foreach (var thread in threads)
                {
                    await Context.Entry(thread).Reference(t => t.UserAccount).LoadAsync();
                    await Context.Entry(thread).Collection(t => t.Messages).LoadAsync();
                }
                var users = await Context.Set<StudentifyAccount>().ToListAsync();
                foreach (var user in users)
                {
                    await Context.Entry(user).Reference(i => i.User).LoadAsync();
                }
            };
        }

        public async Task AddMessageToThread(Thread thread, Message message)
        {
            thread.Messages.Add(message);
            await Context.SaveChangesAsync();
        }
    }
}
