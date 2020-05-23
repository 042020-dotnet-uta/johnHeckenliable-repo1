using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
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

        public async override Task<Order> Add(Order order)
        {
            //Check to make sure there is enough inventory
            foreach (var item in order.ProductsOrdered)
            {
                if (!(CheckForEnoughInventory(order.StoreId, item.ProductId, item.Quantity)))
                    throw new ArgumentOutOfRangeException($"Not enough inventory for product with ID {item.ProductId}");
            }

            #region Decrement Inventory
            //add the order to the db
            _context.Add(order);
            //decrement the appropriate inventories
            foreach (var item in order.ProductsOrdered)
            {
                UpdateLocationQuantity(order.StoreId, item.ProductId, (item.Quantity * -1));
            }
            #endregion

            await _context.SaveChangesAsync();
            return order;
        }

        public override async Task<IEnumerable<Order>> All()
        {
            return await _context.Orders
                .AsNoTracking()
                .Include(o => o.ProductsOrdered)
                .ThenInclude(l => l.Product)
                .Include(o => o.Store)
                .Include(o=>o.Customer)
                .ToListAsync();
        }

        public override async Task<Order> Get(int? id)
        {
            return await _context.Orders
                .AsNoTracking()
                .Include(o => o.ProductsOrdered)
                .ThenInclude(l => l.Product)
                .Include(o => o.Store)
                .Include(o => o.Customer)
                .SingleAsync(o=>o.OrderId == id);
        }

        public override async Task<IEnumerable<Order>> Find(Expression<Func<Order, bool>> predicate)
        {
            return await _context.Orders
                .AsNoTracking()
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

        private bool CheckForEnoughInventory(int storeId, int prodId, int quantity)
        {
            var inventory = (from inv in _context.StoreInventories
                             where inv.StoreId == storeId && inv.ProductId == prodId
                             select inv).AsNoTracking().Take(1).FirstOrDefault();

            return inventory.Quantity >= quantity;
        }

        private void UpdateLocationQuantity(int storeId, int prodId, int quatitiyUpdate)
        {
            var inventory = (from inv in _context.StoreInventories
                             where inv.StoreId == storeId && inv.ProductId == prodId
                             select inv).Take(1).FirstOrDefault();

            inventory.Quantity += quatitiyUpdate;
        }
    }
}
