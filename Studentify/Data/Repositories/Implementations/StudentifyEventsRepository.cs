using Microsoft.EntityFrameworkCore;
using Studentify.Models.StudentifyEvents;

namespace Studentify.Data.Repositories
{
    public class StudentifyEventsRepository : RepositoryBase<StudentifyEvent>, IStudentifyEventsRepository
    {
        public StudentifyEventsRepository(StudentifyDbContext context) : base(context)
        {
        }
    }
}