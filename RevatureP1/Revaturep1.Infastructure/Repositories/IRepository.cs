using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Revaturep1.Infastructure.Repositories
{
    public interface IRepository<T>
    {
        Task<T> Add(T entity);
        Task<T> Update(T entity);
        Task<T> Get(int? id);
        Task<IEnumerable<T>> All();
        Task<IEnumerable<T>> Find(Expression<Func<T, bool>> predicate);
        Task Delete(int? id);
        void SaveChanges();
    }
}
