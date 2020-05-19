using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Revaturep1.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

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
        public async virtual Task<T> Add(T entity)
        {
            var ent = _context.Add(entity).Entity;
            await _context.SaveChangesAsync();
            return ent;
        }

        public async virtual Task<IEnumerable<T>> All()
        {
            return await _context.Set<T>()
                .AsNoTracking()
                .ToListAsync();
        }

        public async virtual Task<IEnumerable<T>> Find(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>()
                .AsNoTracking()
                .AsQueryable()
                .Where(predicate)
                .ToListAsync();
        }

        public async virtual Task<T> Get(int? id)
        {
            return await _context.FindAsync<T>(id);
        }

        public async virtual Task<T> Update(T entity)
        {
            var ent = _context.Update(entity).Entity;
            await _context.SaveChangesAsync();
            return ent;
        }

        public async virtual void Delete(int? id)
        {
            var ent = await Get(id);
            _context.Remove(ent);
            await _context.SaveChangesAsync();
        }
        public async virtual void SaveChanges()
        {
            await _context.SaveChangesAsync();
        }
    }
}
