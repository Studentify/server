using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Studentify.Migrations;

namespace Studentify.Data.Repositories
{
    public interface ISelectRepository<T>
    {
        Task<T> ById(int id);
        Task<IEnumerable<T>> All();
        Func<T, Task> FillWithReferences { get; set; }
    }
}