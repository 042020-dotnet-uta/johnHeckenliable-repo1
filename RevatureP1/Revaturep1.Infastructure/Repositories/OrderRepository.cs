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
    public class OrderRepository : GenericRepository<Order>
    {
        public OrderRepository(ShoppingDbContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<Order>> Find(Expression<Func<Order, bool>> predicate)
        {
            return await _context.Orders
                .Include(o => o.ProductsOrdered)
                .ThenInclude(l => l.Product)
                .Where(predicate)
                .ToListAsync();
        }

        public override async Task<Order> Update(Order entity)
        {
            var order = _context.Orders
                .Include(o => o.ProductsOrdered)
                .SingleAsync(o => o.OrderId == entity.OrderId);

            return await base.Update(entity);
        }
    }
}
