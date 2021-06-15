using Microsoft.EntityFrameworkCore;
using Studentify.Data.Repositories.ControllerRepositories.Interfaces;
using Studentify.Models;
using Studentify.Models.Messages;
using Studentify.Models.StudentifyEvents;
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
        public ThreadsRepository(StudentifyDbContext context,
                               ISelectRepository<Thread> selectRepository,
                               IInsertRepository<Thread> insertRepository,
                               IUpdateRepository<Thread> updateRepository
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
                
                var studentifyEvents = await Context.Set<StudentifyEvent>().ToListAsync();
                foreach (var studentifyEvent in studentifyEvents)
                {
                    await Context.Entry(studentifyEvent).Reference(i => i.Address).LoadAsync();
                }
                
                await Context.Entry(entities).Reference(t => t.ReferencedEvent).LoadAsync();
                await Context.Entry(entities).Collection(t => t.Messages).LoadAsync();
            };
        }

        public async Task<IEnumerable<Thread>> SelectAllUserRelatedThreads(string username)
        {
            var threads = await Select.All();

            return threads.Where(t => ((t.UserAccount.UserName == username) || (t.ReferencedEvent.Author.UserName == username)));
        }
    }
}
