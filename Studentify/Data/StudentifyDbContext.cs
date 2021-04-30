﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Studentify.Models;
using Studentify.Models.Authentication;

namespace Studentify.Data
{
    public class StudentifyDbContext: IdentityDbContext<StudentifyUser>
    {
        public DbSet<Initial> Initial { get; set; }
        public DbSet<StudentifyAccount> StudentifyAccounts { get; set; }
        public DbSet<Event> Events { get; set; }

        public StudentifyDbContext(DbContextOptions<StudentifyDbContext> options) : base(options)
        {
        }
    }
}
