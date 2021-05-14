using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Studentify.Data.Repositories
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected readonly StudentifyDbContext Context;

        protected RepositoryBase(StudentifyDbContext context)
        {
            Context = context;
        }
        
        public async Task RemoveOne(T entity)
        {
            if (entity == null)
            {
                throw new DataException("Type is null");    //todo change to better exception
            }

            Context.Set<T>().Remove(entity);
            await Context.SaveChangesAsync();
        }

        public async Task RemoveById(int id)
        {
            var studentifyEvent = await Context.Set<T>().FindAsync(id);
            if (studentifyEvent == null)
            {
                throw new DataException("Not found"); //todo change to better exception type
            }

            Context.Set<T>().Remove(studentifyEvent);
            await Context.SaveChangesAsync();
        }

        public async Task<T> FindById(int id)
        {
            var info = await Context.Set<T>().FindAsync(id);

            if (info == null)
            {
                throw new DataException("Not found"); //todo change to better exception type
            }

            return info;
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await Context.Set<T>().ToListAsync();
        }
    }
}