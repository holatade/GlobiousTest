using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess.General.Interface
{
    public interface IBaseRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        void Create(T entity);
        Task<int> Save();
        T Find(Func<T, bool> predicate);
        void Update(T entity);
    }
}
