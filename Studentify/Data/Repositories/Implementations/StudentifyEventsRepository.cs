using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Studentify.Models.StudentifyEvents;

namespace Studentify.Data.Repositories
{
    public class StudentifyEventsRepository : RepositoryBase, IStudentifyEventsRepository
    {
        private readonly ISelectRepositoryBase<StudentifyEvent> _selectRepositoryBase;
        private readonly IDeleteRepositoryBase<StudentifyEvent> _deleteRepositoryBase;

        public StudentifyEventsRepository(StudentifyDbContext context,
            ISelectRepositoryBase<StudentifyEvent> selectRepositoryBase,
            IDeleteRepositoryBase<StudentifyEvent> deleteRepositoryBase) : base(context)
        {
            _selectRepositoryBase = selectRepositoryBase;
            _deleteRepositoryBase = deleteRepositoryBase;
            _selectRepositoryBase.FillWithReferences += async entities =>
            {
                await Context.Entry(entities).Reference(i => i.Author).LoadAsync();
                await Context.Entry(entities).Reference(i => i.Address).LoadAsync();
            };
        }

        public async Task<StudentifyEvent> FindById(int id)
        {
            return await _selectRepositoryBase.FindById(id);
        }

        public async Task<IEnumerable<StudentifyEvent>> GetAll()
        {
            return await _selectRepositoryBase.GetAll();
        }

        public async Task RemoveOne(StudentifyEvent entity)
        {
            await _deleteRepositoryBase.RemoveOne(entity);
        }

        public async Task RemoveById(int id)
        {
            await _deleteRepositoryBase.RemoveById(id);
        }
    }
}