using System.Linq;
using System.Threading.Tasks;
using Studentify.Models;
using Studentify.Models.Authentication;

namespace Studentify.Data.Repositories
{
    public class StudentifyAccountsRepository : RepositoryBase, IStudentifyAccountsRepository
    {
        public ISelectRepository<StudentifyAccount> Select { get; set; }
        public IUpdateRepository<StudentifyAccount> Update { get; set; }
        private IInsertRepository<StudentifyAccount> _insert;

        public StudentifyAccountsRepository(StudentifyDbContext context,
            ISelectRepository<StudentifyAccount> selectRepository,
            IUpdateRepository<StudentifyAccount> updateRepository,
            IInsertRepository<StudentifyAccount> insertRepository) : base(context)
        {
            Update = updateRepository;
            Select = selectRepository;
            Select.FillWithReferences += async entities =>
            {
                await Context.Entry(entities).Reference(i => i.User).LoadAsync();
            };
            _insert = insertRepository;
        }

        public async Task<StudentifyAccount> SelectByUsername(string username)
        {
            var accounts = await Select.All();
            return accounts.FirstOrDefault(a => a.User.UserName == username);
        }

        public async Task InsertFromStudentifyUser(StudentifyUser user)
        {
            var account = new StudentifyAccount{StudentifyUserId = user.Id, User = user};
            await _insert.One(account);
        }
    }
}