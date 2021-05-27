using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Studentify.Models;
using Studentify.Models.StudentifyEvents;

namespace Studentify.Data.Repositories.ControllerRepositories.Interfaces
{
    public interface IMeetingsRepository
    {
        public ISelectRepository<Meeting> Select { get; }
        public IInsertRepository<Meeting> Insert { get; }
        public IUpdateRepository<Meeting> Update { get; }
        public Task RegisterAttendance(Meeting meeting, StudentifyAccount account);
    }
}
