using System.Linq;
using System.Threading.Tasks;
using Studentify.Models;

namespace Studentify.Data.Repositories
{
    public interface IStudentifyAccountsRepository
    {
        public ISelectRepository<StudentifyAccount> Select { get; set; }
        public IUpdateRepository<StudentifyAccount> Update { get; set; }
        
        public Task<StudentifyAccount> SelectByUsername(string username);
    }
}