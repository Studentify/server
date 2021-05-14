using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Studentify.Migrations;

namespace Studentify.Data.Repositories
{
    public interface IRepositoryBase<T>
    {
        Task RemoveOne(T entity);
        Task RemoveById(int id);
        Task<T> FindById(int id);
        Task<IEnumerable<T>> GetAll();
    }
}