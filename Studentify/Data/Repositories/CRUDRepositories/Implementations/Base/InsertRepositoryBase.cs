using System.Data;
using System.Threading.Tasks;

namespace Studentify.Data.Repositories
{
    public class InsertRepositoryBase<T> : RepositoryBase, IInsertRepository<T> where T : class
    {
        public InsertRepositoryBase(StudentifyDbContext context) : base(context)
        {
        }

        public async Task One(T entity)
        {
            Context.Set<T>().Add(entity);
            var numberOfAddedEntities = await Context.SaveChangesAsync();
            if (numberOfAddedEntities < 1)
                throw new DataException("Error during insert, no entities added.");
        }
    }
}