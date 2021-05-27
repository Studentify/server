using Studentify.Models.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Studentify.Data.Repositories.ControllerRepositories.Interfaces
{
    public interface IThreadsRepository
    {
        public ISelectRepository<Thread> Select { get; }
        public IInsertRepository<Thread> Insert { get; }
        public IUpdateRepository<Thread> Update { get; }
        public ISelectRepository<Message> SelectMessages { get; }
        public IInsertRepository<Message> InsertMessages { get; }
        public IUpdateRepository<Message> UpdateMessages { get; }
        public Task AddMessageToThread(Thread thread, Message message);
    }
}
