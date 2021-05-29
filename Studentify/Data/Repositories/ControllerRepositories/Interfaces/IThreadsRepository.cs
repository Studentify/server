using Studentify.Models.Messages;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Studentify.Data.Repositories.ControllerRepositories.Interfaces
{
    public interface IThreadsRepository
    {
        public ISelectRepository<Thread> Select { get; }
        public IInsertRepository<Thread> Insert { get; }
        public IUpdateRepository<Thread> Update { get; }
    }
}
