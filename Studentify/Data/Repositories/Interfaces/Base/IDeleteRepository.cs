using System.Threading.Tasks;

namespace Studentify.Data.Repositories
{
    public interface IDeleteRepository<T>
    {
        Task RemoveOne(T entity);
        Task RemoveById(int id);
    }
}