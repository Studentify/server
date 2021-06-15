using Studentify.Models.Messages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Studentify.Data.Repositories.ControllerRepositories.Interfaces
{
    public interface IThreadsRepository
    {
        public ISelectRepository<Thread> Select { get; }
        public IInsertRepository<Thread> Insert { get; }
        public IUpdateRepository<Thread> Update { get; }
        public Task<IEnumerable<Thread>> SelectAllUserRelatedThreads(string username);
    }
}
