using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Revaturep1.Domain.Interfaces
{
    public interface IRepository<T>
    {
        T Add(T entity);
        T Update(T entity);
        T Get(Guid id);
        IEnumerable<T> All();
        IEnumerable<T> Find(Expression<Func<T, bool>> predicate);
        void SaveChanges();
    }
}
