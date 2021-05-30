using Microsoft.EntityFrameworkCore;
using Studentify.Data.Repositories.ControllerRepositories.Interfaces;
using Studentify.Models;
using Studentify.Models.Messages;

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
            };
        }
    }
}
