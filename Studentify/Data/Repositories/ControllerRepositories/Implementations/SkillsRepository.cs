using Microsoft.EntityFrameworkCore;
using Studentify.Data.Repositories.ControllerRepositories.Interfaces;
using Studentify.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Studentify.Data.Repositories.ControllerRepositories.Implementations
{
    public class SkillsRepository : RepositoryBase, ISkillsRepository
    {
        public IInsertRepository<Skill> Insert { get; }
        public IUpdateRepository<Skill> Update { get; }
        public ISelectRepository<Skill> Select { get; set; }
        public SkillsRepository(StudentifyDbContext context,
                               ISelectRepository<Skill> selectRepository,
                               IInsertRepository<Skill> insertRepository,
                               IUpdateRepository<Skill> updateRepository
                               ) : base(context)
        {
            Insert = insertRepository;
            Update = updateRepository;
            Select = selectRepository;

            Select.FillWithReferences += async entities =>
            {
                await Context.Entry(entities).Reference(t => t.Owner).LoadAsync();
                //await Context.Entry(entities).Collection(m => m.).LoadAsync();
                var users = await Context.Set<StudentifyAccount>().ToListAsync();
                foreach (var user in users)
                {
                    await Context.Entry(user).Reference(i => i.User).LoadAsync();
                }
            };
        }

        public async Task UpdateSkillRate(int id, int rate)
        {
            var skill = await Select.ById(id);

            if(skill != null)
            {
                skill.Rate = rate;
            }

            await Context.SaveChangesAsync();
        }
    }
}
