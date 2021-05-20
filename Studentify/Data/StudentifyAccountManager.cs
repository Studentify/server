using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Studentify.Models;

namespace Studentify.Data
{
    public class StudentifyAccountManager
    {
        private readonly StudentifyDbContext _context;

        public StudentifyAccountManager(StudentifyDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<StudentifyAccount>> GetAccountsAsync()
        {
            var studentifyAccounts = await _context.StudentifyAccounts.ToListAsync();
            foreach (var studentifyAccount in studentifyAccounts)
            {
                //await _context.Entry(studentifyAccount).Collection(s => s.Events).LoadAsync();
                await _context.Entry(studentifyAccount).Reference(s => s.User).LoadAsync();
            }

            return studentifyAccounts.ToList();
        }

        public async Task<StudentifyAccount> FindAccountByUsername(string username)
        {
            var studentifyAccounts = await GetAccountsAsync();
            return studentifyAccounts.Where(a => a.User.UserName == username).ToList()[0];
        }
    }
}
