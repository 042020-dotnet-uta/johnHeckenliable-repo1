using Revaturep1.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Revaturep1.DataAccess.Repositories
{
    public abstract class GenericRepository<T>
        : IRepository<T> where T : class
    {
        protected ShoppingDbContext _context;
        public GenericRepository(ShoppingDbContext context)
        {
            this._context = context;
        }
        public virtual T Add(T entity)
        {
            return _context.Add(entity).Entity;
        }

        public virtual IEnumerable<T> All()
        {
            return _context.Set<T>()
                .ToList();
        }

        public virtual IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>()
                .AsQueryable()
                .Where(predicate)
                .ToList();
        }

        public virtual T Get(Guid id)
        {
            return _context.Find<T>(id);
        }

        public virtual T Update(T entity)
        {
            return _context.Update(entity).Entity;
        }
        public virtual void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
