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
using Microsoft.CodeAnalysis.CSharp.Syntax;
using RevatureP1.Web.Models;

namespace RevatureP1.Web.Controllers
{
    public class OrdersController : Controller
    {
        private readonly IRepository<Order> orderRepo;
        private readonly IRepository<Product> productRepo;
        private readonly IRepository<Customer> customerRepo;
        private readonly IRepository<Store> storeRepo;

        public OrdersController(IRepository<Order> orderRepo,
            IRepository<Product> productRepo,
            IRepository<Customer> customerRepo,
            IRepository<Store> storeRepo)
        {
            this.orderRepo = orderRepo;
            this.productRepo = productRepo;
            this.customerRepo = customerRepo;
            this.storeRepo = storeRepo;
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
            var orderViews = new List<OrderViewModel>();
            var orders = await orderRepo.All();
            foreach (var order in orders)
            {
                orderViews.Add(new OrderViewModel
                {
                    Order = order
                });
            }

            return View(orderViews);
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var order = await _context.Orders
            //    .Include(o => o.Customer)
            //    .Include(o => o.Store)
            //    .FirstOrDefaultAsync(m => m.OrderId == id);

            var order = await orderRepo.Get(id);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            ViewData["CusomerId"] = new SelectList(customerRepo.All().Result, "CustomerId", "CustomerId");
            ViewData["StoreId"] = new SelectList(storeRepo.All().Result, "StoreId", "StoreId");
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderId,CusomerId,StoreId,OrderDateTime")] Order order)
        {
            if (ModelState.IsValid)
            {
                //_context.Add(order);
                //await _context.SaveChangesAsync();
                order = await orderRepo.Add(order);

                return RedirectToAction(nameof(Index));
            }
            ViewData["CusomerId"] = new SelectList(customerRepo.All().Result, "CustomerId", "CustomerId", order.CusomerId);
            ViewData["StoreId"] = new SelectList(storeRepo.All().Result, "StoreId", "StoreId", order.StoreId);
            return View(order);
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await orderRepo.Get(id);//_context.Orders.FindAsync(id);

            if (order == null)
            {
                return NotFound();
            }
            ViewData["CusomerId"] = new SelectList(customerRepo.All().Result, "CustomerId", "CustomerId", order.CusomerId);
            ViewData["StoreId"] = new SelectList(storeRepo.All().Result, "StoreId", "StoreId", order.StoreId);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderId,CusomerId,StoreId,OrderDateTime")] Order order)
        {
            if (id != order.OrderId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                    //_context.Update(order);
                    //await _context.SaveChangesAsync();
                    order = await orderRepo.Update(order);
                /*
                try
                {

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.OrderId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                */
                return RedirectToAction(nameof(Index));
            }
            ViewData["CusomerId"] = new SelectList(customerRepo.All().Result, "CustomerId", "CustomerId", order.CusomerId);
            ViewData["StoreId"] = new SelectList(storeRepo.All().Result, "StoreId", "StoreId", order.StoreId);
            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var order = await _context.Orders
            //    .Include(o => o.Customer)
            //    .Include(o => o.Store)
            //    .FirstOrDefaultAsync(m => m.OrderId == id);
            var order = await orderRepo.Get(id);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            //var order = await _context.Orders.FindAsync(id);
            //_context.Orders.Remove(order);
            //await _context.SaveChangesAsync();
            await orderRepo.Delete(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
