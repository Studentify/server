using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Studentify.Models.StudentifyEvents;

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

            await FillWithReferences(info);

            return info;
        }

        public virtual async Task<IEnumerable<T>> GetAll()
        {
            var entities = await Context.Set<T>().ToListAsync();
            foreach (var entity in entities)
            {
                await FillWithReferences(entity);
            }
            return entities;
        }

        protected abstract Task FillWithReferences(T entities);
    }
}