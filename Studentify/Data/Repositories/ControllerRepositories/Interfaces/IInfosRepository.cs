using Studentify.Models.StudentifyEvents;

namespace Studentify.Data.Repositories
{
    public interface IInfosRepository
    {
        public ISelectRepository<Info> Select { get; }
        public IInsertRepository<Info> Insert { get; }
    }
}