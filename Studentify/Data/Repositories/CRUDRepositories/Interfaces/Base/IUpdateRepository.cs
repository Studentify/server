using System.Threading.Tasks;

namespace Studentify.Data.Repositories
{
    public interface IUpdateRepository<T>
    {
        Task One(T entity, int id);
    }
}