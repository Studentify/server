using Microsoft.EntityFrameworkCore;
using Studentify.Models;
using Studentify.Models.Messaging;

namespace Studentify.Data
{
    public class StudentifyDbContext: DbContext
    {
        public DbSet<Initial> Initial { get; set; }
        public DbSet<Message> Message { get; set; }

        public StudentifyDbContext(DbContextOptions<StudentifyDbContext> options) : base(options)
        {
        }
    }
}