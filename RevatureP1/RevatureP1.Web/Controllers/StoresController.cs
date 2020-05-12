using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RevatureP1.Models;
using Revaturep1.DataAccess;
using Revaturep1.Domain.Interfaces;
using RevatureP1.Web.Models;

namespace RevatureP1.Web.Controllers
{
    public class StoresController : Controller
    {
        private readonly ShoppingDbContext _context;
        private readonly IRepository<Order> orderRepo;

        public StoresController(ShoppingDbContext context,
            IRepository<Order> orderRepo)
        {
            _context = context;
            this.orderRepo = orderRepo;
        }

        // GET: Stores
        public async Task<IActionResult> Index()
        {
            return View(await _context.Stores.ToListAsync());
        }

        // GET: Stores/Details/5
        public async Task<IActionResult> AvailableProducts(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var store = await _context.Stores
                .Include(s=>s.AvailableProducts)
                .ThenInclude(p=>p.Product)
                .FirstOrDefaultAsync(m => m.StoreId == id);
            if (store == null)
            {
                return NotFound();
            }

            return View(store);
        }

        // GET: Stores/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Stores/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StoreId,Location")] Store store)
        {
            if (ModelState.IsValid)
            {
                _context.Add(store);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(store);
        }


        // GET: Stores/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var store = await _context.Stores.FindAsync(id);
            if (store == null)
            {
                return NotFound();
            }
            return View(store);
        }

        // POST: Stores/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StoreId,Location")] Store store)
        {
            if (id != store.StoreId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(store);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StoreExists(store.StoreId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(store);
        }

        // GET: Stores/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var store = await _context.Stores
                .FirstOrDefaultAsync(m => m.StoreId == id);
            if (store == null)
            {
                return NotFound();
            }

            return View(store);
        }

        // POST: Stores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var store = await _context.Stores.FindAsync(id);
            _context.Stores.Remove(store);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> OrderHistory(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var orders = await orderRepo.Find(o => o.StoreId == id);
            if (orders == null)
            {
                return NotFound();
            }
            var orderViews = new List<OrderViewModel>();
            foreach (var order in orders)
            {
                orderViews.Add(new OrderViewModel
                {
                    Order = order
                });
            }

            return View(orderViews);
        }
        public async Task<IActionResult> OrderDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var order = await orderRepo.Get(id);

            if (order == null)
            {
                return NotFound();
            }
            var orderDetails = new OrderDetailsViewModel
            {
                OrderId = order.OrderId,
                Customer = order.Customer,
                Store = order.Store,
                OrderDateTime = order.OrderDateTime
            };
            foreach (var item in order.ProductsOrdered)
            {
                orderDetails.LineItems.Add(
                    new LineItemViewModel
                    {
                        OrderDetails = item
                    });
            }

            return View(orderDetails);
        }

        private bool StoreExists(int id)
        {
            return _context.Stores.Any(e => e.StoreId == id);
        }
    }
}
