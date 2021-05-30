using Studentify.Models.Messages;

namespace Studentify.Data.Repositories.ControllerRepositories.Interfaces
{
    public interface IThreadsRepository
    {
        public ISelectRepository<Thread> Select { get; }
        public IInsertRepository<Thread> Insert { get; }
        public IUpdateRepository<Thread> Update { get; }
    }
}
