using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Studentify.Data.Repositories
{
    public class UpdateRepositoryBase<T> : RepositoryBase, IUpdateRepository<T>
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
                if (!InfoExists(id))
                    throw new DataException("Not found"); //todo change to better exception type
                throw;
            }
        }
        
        private bool InfoExists(int id)
        {
            return Context.Infos.Any(e => e.Id == id);
        }
    }
}