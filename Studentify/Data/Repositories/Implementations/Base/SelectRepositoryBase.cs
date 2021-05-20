using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Studentify.Models.StudentifyEvents;

namespace Studentify.Data.Repositories
{
    public class SelectRepositoryBase<T> : RepositoryBase, ISelectRepositoryBase<T> where T : class
    {
        public SelectRepositoryBase(StudentifyDbContext context) : base(context)
        {
        }

        public async Task<T> FindById(int id)
        {
            var info = await Context.Set<T>().FindAsync(id);

            if (info == null)
            {
                throw new DataException("Not found"); //todo change to better exception type
            }

            
            await FillWithReferences(info);

            return info;
        }

        public async Task<IEnumerable<T>> GetAll()
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