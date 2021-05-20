using Studentify.Models.StudentifyEvents;

namespace Studentify.Data.Repositories
{
    public interface IStudentifyEventsRepository : ISelectRepository<StudentifyEvent>, IDeleteRepository<StudentifyEvent>
    {
    }
}