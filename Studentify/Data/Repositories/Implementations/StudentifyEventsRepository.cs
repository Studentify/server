using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Studentify.Models.StudentifyEvents;

namespace Studentify.Data.Repositories
{
    public class StudentifyEventsRepository : RepositoryBase<StudentifyEvent>, IStudentifyEventsRepository
    {
        public StudentifyEventsRepository(StudentifyDbContext context) : base(context)
        {
        }


        protected override async Task FillWithReferences(StudentifyEvent entities)
        {
            await Context.Entry(entities).Reference(i => i.Author).LoadAsync();
            await Context.Entry(entities).Reference(i => i.Address).LoadAsync(); 
        }
    }
}