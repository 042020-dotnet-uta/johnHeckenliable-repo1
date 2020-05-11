using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
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

        public override async Task<IEnumerable<Order>> All()
        {
            return await _context.Orders
                .Include(o => o.ProductsOrdered)
                .ThenInclude(l => l.Product)
                .Include(o => o.Store)
                .Include(o=>o.Customer)
                .ToListAsync();
        }

        public override async Task<Order> Get(int? id)
        {
            return await _context.Orders
                .Include(o => o.ProductsOrdered)
                .ThenInclude(l => l.Product)
                .Include(o => o.Store)
                .Include(o => o.Customer)
                .SingleAsync(o=>o.OrderId == id);
        }

        public override async Task<IEnumerable<Order>> Find(Expression<Func<Order, bool>> predicate)
        {
            return await _context.Orders
                .Include(o => o.ProductsOrdered)
                .ThenInclude(l => l.Product)
                .Include(o => o.Store)
                .Include(o => o.Customer)
                .Where(predicate)
                .ToListAsync();
        }

        public override async Task<Order> Update(Order entity)
        {
            var order = _context.Orders
                .Include(o => o.ProductsOrdered)
                .Include(o => o.Store)
                .Include(o => o.Customer)
                .SingleAsync(o => o.OrderId == entity.OrderId);

            return await base.Update(entity);
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.OrderId == id);
        }
    }
}
