using System.Data;
using System.Threading.Tasks;

namespace Studentify.Data.Repositories
{
    public class DeleteRepositoryBase<T> : RepositoryBase, IDeleteRepository<T> where T : class
    {
        public DeleteRepositoryBase(StudentifyDbContext context) : base(context)
        {
        }

        public async Task One(T entity)
        {
            if (entity == null)
            {
                throw new DataException("Type is null");    //todo change to better exception
            }

            Context.Set<T>().Remove(entity);
            await Context.SaveChangesAsync();
        }

        public async Task ById(int id)
        {
            var studentifyEvent = await Context.Set<T>().FindAsync(id);
            if (studentifyEvent == null)
            {
                throw new DataException("Not found"); //todo change to better exception type
            }

            Context.Set<T>().Remove(studentifyEvent);
            await Context.SaveChangesAsync();
        }
    }
}