using Studentify.Models.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Studentify.Data.Repositories.ControllerRepositories.Interfaces
{
    public interface IMessagesRepository
    {
        public ISelectRepository<Message> Select { get; }
        public IInsertRepository<Message> Insert { get; }
        public IUpdateRepository<Message> Update { get; }
        public Task InsertMessageToThread(Message message, Thread thread);
        public Task<IEnumerable<Message>> SelectAllFromThread(int threadId);
    }
}
