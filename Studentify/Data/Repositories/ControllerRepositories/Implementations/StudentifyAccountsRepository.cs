using System.Linq;
using System.Threading.Tasks;
using Studentify.Models;

namespace Studentify.Data.Repositories
{
    public class StudentifyAccountsRepository : RepositoryBase, IStudentifyAccountsRepository
    {
        public ISelectRepository<StudentifyAccount> Select { get; set; }
        public IUpdateRepository<StudentifyAccount> Update { get; set; }

        public StudentifyAccountsRepository(StudentifyDbContext context,
            ISelectRepository<StudentifyAccount> selectRepository,
            IUpdateRepository<StudentifyAccount> updateRepository) : base(context)
        {
            Update = updateRepository;
            Select = selectRepository;
            Select.FillWithReferences += async entities =>
            {
                await Context.Entry(entities).Reference(i => i.User).LoadAsync();
            };
        }

        public async Task<StudentifyAccount> SelectByUsername(string username)
        {
            var accounts = await Select.All();
            return accounts.FirstOrDefault(a => a.User.UserName == username);
        }
    }
}