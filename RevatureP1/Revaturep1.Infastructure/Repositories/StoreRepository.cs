using Microsoft.EntityFrameworkCore;
using RevatureP1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Revaturep1.DataAccess.Repositories
{
    public class StoreRepository : GenericRepository<Store>
    {
        public StoreRepository(ShoppingDbContext context) 
            : base(context)
        {
        }

        public override async Task<Store> Get(int? id)
        {
            return await _context.Stores
                .AsNoTracking()
                .Include(s => s.AvailableProducts)
                .ThenInclude(a => a.Product)
                .Where(s=>s.StoreId == id)
                .SingleAsync();
        }
        public override async Task<IEnumerable<Store>> Find(Expression<Func<Store, bool>> predicate)
        {
            return await _context.Stores
                .AsNoTracking()
                .Include(s => s.AvailableProducts)
                .ThenInclude(a => a.Product)
                .Where(predicate)
                .ToListAsync();
        }

        public override async Task<Store> Update(Store entity)
        {
            var order = await _context.Stores
                .Include(s => s.AvailableProducts)
                .SingleAsync(s => s.StoreId == entity.StoreId);

            return await base.Update(entity);
        }
    }
}
