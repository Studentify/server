using Studentify.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Studentify.Data.Repositories.ControllerRepositories.Interfaces
{
    public interface ISkillsRepository
    {
        public ISelectRepository<Skill> Select { get; }
        public IInsertRepository<Skill> Insert { get; }
        public IUpdateRepository<Skill> Update { get; }
        public Task UpdateSkillRate(int id, int rate);
    }
}
