using Microsoft.EntityFrameworkCore;
using Revaturep1.DataAccess;
using Revaturep1.DataAccess.Repositories;
using RevatureP1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RevatureP1.DataAccess.Repositories
{
    public class InventoryRepository : GenericRepository<Inventory>
    {
        public InventoryRepository(ShoppingDbContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<Inventory>> Find(Expression<Func<Inventory, bool>> predicate)
        {
            return await _context.StoreInventories
                .Include(i => i.Product)
                .Where(predicate)
                .ToListAsync();
        }
    }
}
