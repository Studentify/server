using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Studentify.Data.Repositories
{
    public class SelectRepositoryBase<T> : RepositoryBase, ISelectRepository<T> where T : class
    {
        public SelectRepositoryBase(StudentifyDbContext context) : base(context)
        {
        }

        public async Task<T> ById(int id)
        {
            var info = await Context.Set<T>().FindAsync(id);

            if (info == null)
            {
                throw new DataException("Not found"); //todo change to better exception type
            }

            
            await FillWithReferences(info);

            return info;
        }

        public async Task<IEnumerable<T>> All()
        {
            var entities = await Context.Set<T>().ToListAsync();
            foreach (var entity in entities)
            {
                await FillWithReferences(entity);
            }
            return entities;
        }

        public Func<T, Task> FillWithReferences { get; set; }
    }
}