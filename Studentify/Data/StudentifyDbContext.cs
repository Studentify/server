﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Studentify.Models;
using Studentify.Models.Authentication;
using Studentify.Models.StudentifyEvents;

namespace Studentify.Data
{
    public class StudentifyDbContext: IdentityDbContext<StudentifyUser>
    {
        public DbSet<StudentifyAccount> StudentifyAccounts { get; set; }
        public DbSet<StudentifyEvent> Events { get; set; }
        public DbSet<Info> Infos { get; set; }
        public DbSet<Meeting> Meetings { get; set; }

        public StudentifyDbContext(DbContextOptions<StudentifyDbContext> options) : base(options)
        {
        }
    }
}
