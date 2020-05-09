using Microsoft.EntityFrameworkCore;
using RevatureP1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Revaturep1.Infastructure.Repositories
{
    public class OrderRepository : GenericRepository<Order>
    {
        public OrderRepository(ShoppingDbContext context) : base(context)
        {
        }

        public override IEnumerable<Order> Find(Expression<Func<Order, bool>> predicate)
        {
            return _context.Orders
                .Include(o => o.ProductsOrdered)
                .ThenInclude(l => l.Product)
                .Where(predicate)
                .ToList();
        }

        public override Order Update(Order entity)
        {
            var order = _context.Orders
                .Include(o => o.ProductsOrdered)
                .Single(o => o.OrderId == entity.OrderId);

            return base.Update(entity);
        }
    }
}
