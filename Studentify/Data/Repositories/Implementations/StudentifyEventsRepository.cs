using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Studentify.Models.StudentifyEvents;

namespace Studentify.Data.Repositories
{
    public class StudentifyEventsRepository : RepositoryBase, IStudentifyEventsRepository
    {
        public ISelectRepository<StudentifyEvent> Select { get; }
        public IDeleteRepository<StudentifyEvent> Delete { get; }

        public StudentifyEventsRepository(StudentifyDbContext context,
            ISelectRepository<StudentifyEvent> selectRepository,
            IDeleteRepository<StudentifyEvent> deleteRepository) : base(context)
        {
            Select = selectRepository;
            Delete = deleteRepository;
            Select.FillWithReferences += async entities =>
            {
                await Context.Entry(entities).Reference(i => i.Author).LoadAsync();
                await Context.Entry(entities).Reference(i => i.Address).LoadAsync();
            };
        }
    }
}