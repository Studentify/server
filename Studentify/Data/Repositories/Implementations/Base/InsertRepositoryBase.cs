using System;
using System.Collections.Generic;
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
            await Context.SaveChangesAsync();
        }
    }
}