using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Studentify.Models.StudentifyEvents;

namespace Studentify.Data.Repositories
{
    public class StudentifyEventsRepository : RepositoryBase, IStudentifyEventsRepository
    {
        private readonly ISelectRepository<StudentifyEvent> _selectRepository;
        private readonly IDeleteRepository<StudentifyEvent> _deleteRepository;

        public StudentifyEventsRepository(StudentifyDbContext context,
            ISelectRepository<StudentifyEvent> selectRepository,
            IDeleteRepository<StudentifyEvent> deleteRepository) : base(context)
        {
            _selectRepository = selectRepository;
            _deleteRepository = deleteRepository;
            _selectRepository.FillWithReferences += async entities =>
            {
                await Context.Entry(entities).Reference(i => i.Author).LoadAsync();
                await Context.Entry(entities).Reference(i => i.Address).LoadAsync();
            };
        }

        public async Task<StudentifyEvent> FindById(int id)
        {
            return await _selectRepository.FindById(id);
        }

        public async Task<IEnumerable<StudentifyEvent>> GetAll()
        {
            return await _selectRepository.GetAll();
        }

        public async Task RemoveOne(StudentifyEvent entity)
        {
            await _deleteRepository.RemoveOne(entity);
        }

        public async Task RemoveById(int id)
        {
            await _deleteRepository.RemoveById(id);
        }

        public Func<StudentifyEvent, Task> FillWithReferences { get; set; }
    }
}