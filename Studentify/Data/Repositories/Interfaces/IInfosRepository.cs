using Studentify.Models.StudentifyEvents;

namespace Studentify.Data.Repositories
{
    public interface IInfosRepository : IStudentifyEventsRepository
    {
        public IInsertRepository<Info> Insert { get; }
    }
}