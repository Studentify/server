using System.Data;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Studentify.Data.Repositories
{
    public class UpdateRepositoryBase<T> : RepositoryBase, IUpdateRepository<T> where T : class
    {
        public UpdateRepositoryBase(StudentifyDbContext context) : base(context)
        {
        }
        
        public async Task One(T entity, int id)
        {
            Context.Entry(entity).State = EntityState.Modified;

            try
            {
                await Context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await EntityExists(id))
                    throw new DataException("Not found"); //todo change to better exception type
                throw;
            }
        }
        
        private async Task<bool> EntityExists(int id)
        {
            return await Context.Set<T>().FindAsync() == null;
        }
    }
}