using System.Collections.Generic;
using System.Threading.Tasks;
using Studentify.Models;
using Studentify.Models.Authentication;
using Studentify.Models.DTO;

namespace Studentify.Data.Repositories
{
    public interface IStudentifyAccountsRepository
    {
        public ISelectRepository<StudentifyAccount> Select { get; set; }
        public IUpdateRepository<StudentifyAccount> Update { get; set; }
        
        public Task<StudentifyAccount> SelectByUsername(string username);
        public Task InsertFromStudentifyUser(StudentifyUser user);
        public Task<IEnumerable<Skill>> GetSkills(int accountId);
        public Task SaveSkill(int accountId, Skill skill);
    }
}