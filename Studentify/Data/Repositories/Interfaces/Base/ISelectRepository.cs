using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Studentify.Data.Repositories
{
    public interface ISelectRepository<T>
    {
        Task<T> ById(int id);
        Task<IEnumerable<T>> All();
        Func<T, Task> FillWithReferences { get; set; }
    }
}