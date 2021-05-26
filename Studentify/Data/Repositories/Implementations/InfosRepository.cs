using Studentify.Models.StudentifyEvents;

namespace Studentify.Data.Repositories
{
    public class InfosRepository : StudentifyEventsRepository, IInfosRepository
    {
        public IInsertRepository<Info> Insert { get; }
        
        public InfosRepository(StudentifyDbContext context, ISelectRepository<StudentifyEvent> selectRepository,
            IDeleteRepository<StudentifyEvent> deleteRepository, IInsertRepository<Info> insertRepository) : base(
            context, selectRepository, deleteRepository)
        {
            Insert = insertRepository;
        }
    }
}