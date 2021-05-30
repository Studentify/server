using System.Threading.Tasks;

namespace Studentify.Data.Repositories
{
    public interface IDeleteRepository<T>
    {
        Task One(T entity);
        Task ById(int id);
    }
}