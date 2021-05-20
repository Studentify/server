using System.Threading.Tasks;
using Studentify.Models.StudentifyEvents;

namespace Studentify.Data.Repositories
{
    public class InfosRepository : StudentifyEventsRepository, IInfosRepository
    {
        private readonly IInsertRepository<Info> _insertRepository;

        public InfosRepository(StudentifyDbContext context, ISelectRepository<StudentifyEvent> selectRepository,
            IDeleteRepository<StudentifyEvent> deleteRepository, IInsertRepository<Info> insertRepository) : base(
            context, selectRepository, deleteRepository)
        {
            _insertRepository = insertRepository;
        }

        public Task InsertOne(Info entity)
        {
            return _insertRepository.InsertOne(entity);
        }
    }
}