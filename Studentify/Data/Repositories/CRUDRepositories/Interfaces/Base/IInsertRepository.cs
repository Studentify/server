using System.Threading.Tasks;

namespace Studentify.Data.Repositories
{
    public interface IInsertRepository<T>
    {
        Task One(T entity);
    }
}