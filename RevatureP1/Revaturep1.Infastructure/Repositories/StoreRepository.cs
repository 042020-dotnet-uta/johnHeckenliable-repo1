using Microsoft.EntityFrameworkCore;
using RevatureP1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Revaturep1.DataAccess.Repositories
{
    public class StoreRepository : GenericRepository<Store>
    {
        public StoreRepository(ShoppingDbContext context) 
            : base(context)
        {
        }
        public override IEnumerable<Store> Find(Expression<Func<Store, bool>> predicate)
        {
            return _context.Stores
                .Include(s => s.AvailableProducts)
                .ThenInclude(a => a.Product)
                .Where(predicate)
                .ToList();
        }

        public override Store Update(Store entity)
        {
            var order = _context.Stores
                .Include(s => s.AvailableProducts)
                .Single(s => s.StoreId == entity.StoreId);

            return base.Update(entity);
        }
    }
}
