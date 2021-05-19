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


        protected override async Task<IEnumerable<StudentifyEvent>> FillWithReferences(IEnumerable<StudentifyEvent> elems)
        {
            var fillWithReferences = elems.ToList();
            foreach (var elem in fillWithReferences)
            {
                await Context.Entry(elem).Reference(i => i.Author).LoadAsync();
                await Context.Entry(elem).Reference(i => i.Address).LoadAsync(); 
            }

            return fillWithReferences;
        }
    }
}