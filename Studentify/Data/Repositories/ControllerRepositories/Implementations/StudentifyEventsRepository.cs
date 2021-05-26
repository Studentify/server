using Studentify.Models.StudentifyEvents;

namespace Studentify.Data.Repositories
{
    public class StudentifyEventsRepository : StudentifyEventRepositorySelectBase<StudentifyEvent>, IStudentifyEventsRepository
    {
        public IDeleteRepository<StudentifyEvent> Delete { get; }
        public StudentifyEventsRepository(StudentifyDbContext context,
            ISelectRepository<StudentifyEvent> selectRepository,
            IDeleteRepository<StudentifyEvent> deleteRepository) : base(context, selectRepository)
        {
            Delete = deleteRepository;
        }
    }
}