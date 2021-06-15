using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Studentify.Models;
using Studentify.Models.StudentifyEvents;

namespace Studentify.Data.Repositories
{
    public abstract class StudentifyEventRepositorySelectBase<T> : RepositoryBase where T : StudentifyEvent
    {
        public ISelectRepository<T> Select { get; }
        public StudentifyEventRepositorySelectBase(StudentifyDbContext context, ISelectRepository<T> selectRepository) : base(context)
        {
            Select = selectRepository;
            Select.FillWithReferences += FillWithReferences;
        }

        protected virtual async Task FillWithReferences(T entities)
        {
            await Context.Entry(entities).Reference(i => i.Author).LoadAsync();
            var users = await Context.Set<StudentifyAccount>().ToListAsync();
            foreach (var user in users)
            {
                await Context.Entry(user).Reference(i => i.User).LoadAsync();
            }
            await Context.Entry(entities).Reference(i => i.Address).LoadAsync();
        }
    }
}