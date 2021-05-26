using Studentify.Models.StudentifyEvents;

namespace Studentify.Data.Repositories
{
    public interface IStudentifyEventsRepository
    {
        public ISelectRepository<StudentifyEvent> Select { get; }
        public IDeleteRepository<StudentifyEvent> Delete { get; }
    }
}