using System.Threading.Tasks;
using Studentify.Models.StudentifyEvents;

namespace Studentify.Data.Repositories
{
    public class InfosRepository : StudentifyEventsRepository, IInfosRepository
    {
        private readonly IInsertRepositoryBase<Info> _insertRepositoryBase;

        public InfosRepository(StudentifyDbContext context, ISelectRepositoryBase<StudentifyEvent> selectRepositoryBase,
            IDeleteRepositoryBase<StudentifyEvent> deleteRepositoryBase, IInsertRepositoryBase<Info> insertRepositoryBase) : base(
            context, selectRepositoryBase, deleteRepositoryBase)
        {
            _insertRepositoryBase = insertRepositoryBase;
        }

        public Task InsertOne(Info entity)
        {
            return _insertRepositoryBase.InsertOne(entity);
        }
    }
}