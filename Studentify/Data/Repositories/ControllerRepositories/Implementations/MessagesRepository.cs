using Microsoft.EntityFrameworkCore;
using Studentify.Data.Repositories.ControllerRepositories.Interfaces;
using Studentify.Models;
using Studentify.Models.Messages;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Studentify.Data.Repositories.ControllerRepositories.Implementations
{
    public class MessagesRepository : RepositoryBase, IMessagesRepository
    {
        public IInsertRepository<Message> Insert { get; }
        public IUpdateRepository<Message> Update { get; }
        public ISelectRepository<Message> Select { get; set; }

        public MessagesRepository(StudentifyDbContext context,
                               ISelectRepository<Message> selectRepository,
                               IInsertRepository<Message> insertRepository,
                               IUpdateRepository<Message> updateRepository
                               ) : base(context)
        {
            Insert = insertRepository;
            Update = updateRepository;
            Select = selectRepository;

            Select.FillWithReferences += async entities =>
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

        public async Task InsertMessageToThread(Message message, Thread thread)
        {
            await Context.Messages.AddAsync(message);
            thread.Messages.Add(message);
            await Context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Message>> SelectAllFromThread(int threadId)
        {
            var messages = await Select.All();
            return messages.Where(m => m.ThreadId == threadId).ToList();
        }
    }
}
