using Studentify.Models.StudentifyEvents;

namespace Studentify.Data.Repositories
{
    public class InfosRepository : StudentifyEventRepositorySelectBase<Info>, IInfosRepository
    {
        public IInsertRepository<Info> Insert { get; }
        public InfosRepository(StudentifyDbContext context, 
                               ISelectRepository<Info> selectRepository,
                               IInsertRepository<Info> insertRepository
                               ) : base(context, selectRepository)
        {
            Insert = insertRepository;
        }
    }
}