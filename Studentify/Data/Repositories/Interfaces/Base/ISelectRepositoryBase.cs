using System;
using System.Threading.Tasks;

namespace Studentify.Data.Repositories
{
    public interface ISelectRepositoryBase<T> : ISelectRepository<T>
    {
        Func<T, Task> FillWithReferences { get; set; }
    }
}