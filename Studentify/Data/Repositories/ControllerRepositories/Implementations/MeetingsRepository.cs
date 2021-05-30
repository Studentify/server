using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Studentify.Data.Repositories.ControllerRepositories.Interfaces;
using Studentify.Models;
using Studentify.Models.StudentifyEvents;

namespace Studentify.Data.Repositories.ControllerRepositories.Implementations
{
    public class MeetingsRepository : StudentifyEventRepositorySelectBase<Meeting>, IMeetingsRepository
    {
        public IInsertRepository<Meeting> Insert { get; }
        public IUpdateRepository<Meeting> Update { get; }
        public MeetingsRepository(StudentifyDbContext context,
            ISelectRepository<Meeting> selectRepository,
            IInsertRepository<Meeting> insertRepository,
            IUpdateRepository<Meeting> updateRepository
        ) : base(context, selectRepository)
        {
            Insert = insertRepository;
            Update = updateRepository;
            Select.FillWithReferences += async entities =>
            {
                await Context.Entry(entities).Collection(m => m.Participants).LoadAsync();
                var users = await Context.Set<StudentifyAccount>().ToListAsync();
                foreach (var user in users)
                {
                    await Context.Entry(user).Reference(i => i.User).LoadAsync();
                }
            };
        }

        public async Task RegisterAttendance(Meeting meeting, StudentifyAccount account)
        {
            meeting.Participants.Add(account);
            await Context.SaveChangesAsync();
        }
    }
}
