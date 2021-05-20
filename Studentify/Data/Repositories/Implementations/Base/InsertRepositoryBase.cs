using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Studentify.Data.Repositories
{
    public class InsertRepositoryBase<T> : RepositoryBase, IInsertRepositoryBase<T> where T : class
    {
        public InsertRepositoryBase(StudentifyDbContext context) : base(context)
        {
        }

        public async Task InsertOne(T entity)
        {
            Context.Set<T>().Add(entity);
            await Context.SaveChangesAsync();
        }
    }
}