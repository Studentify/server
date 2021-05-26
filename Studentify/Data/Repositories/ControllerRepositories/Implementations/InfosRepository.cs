using Studentify.Models.StudentifyEvents;

namespace Studentify.Data.Repositories
{
    public class InfosRepository : StudentifyEventRepositorySelectBase<Info>, IInfosRepository
    {
        public IInsertRepository<Info> Insert { get; }
        public IUpdateRepository<Info> Update { get; }
        public InfosRepository(StudentifyDbContext context, 
                               ISelectRepository<Info> selectRepository,
                               IInsertRepository<Info> insertRepository,
                               IUpdateRepository<Info> updateRepository
                               ) : base(context, selectRepository)
        {
            Insert = insertRepository;
            Update = updateRepository;
        }
    }
}

