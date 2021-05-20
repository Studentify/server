using Studentify.Models.StudentifyEvents;

namespace Studentify.Data.Repositories
{
    public interface IInfosRepository : IStudentifyEventsRepository, IInsertRepository<Info>
    {
    }
}