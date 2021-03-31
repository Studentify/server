using Microsoft.EntityFrameworkCore;
using Studentify.Models;

namespace Studentify.Data
{
    public class StudentifyDbContext: DbContext
    {
        public DbSet<Initial> Initial { get; set; }

        public StudentifyDbContext(DbContextOptions<StudentifyDbContext> options) : base(options)
        {
        }
    }
}
