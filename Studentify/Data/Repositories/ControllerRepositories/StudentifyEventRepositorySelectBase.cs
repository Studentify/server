using System.Threading.Tasks;
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
            await Context.Entry(entities).Reference(i => i.Address).LoadAsync();
        }
    }
}