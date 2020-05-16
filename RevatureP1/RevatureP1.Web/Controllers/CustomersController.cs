using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RevatureP1.Models;
using Revaturep1.DataAccess;
using Revaturep1.DataAccess.Repositories;
using Revaturep1.Domain.Interfaces;
using RevatureP1.Web.Models;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Authorization;
using RevatureP1.Web.Helpers;
using System.Reflection.Metadata.Ecma335;

namespace RevatureP1.Web.Controllers
{
    public class CustomersController : Controller
    {
        private readonly IRepository<Customer> customerRepo;
        private readonly IRepository<Order> orderRepo;

        public CustomersController(IRepository<Customer> customerRepo,
            IRepository<Order> orderRepo)
        {
            this.customerRepo = customerRepo;
            this.orderRepo = orderRepo;
        }

        // GET: Customers
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index(int? SearchType, string searchString)
        {
            IEnumerable<Customer> customers;
            if (!String.IsNullOrEmpty(searchString))
            {
                //Build a predicate to send to the repository to search
                var predicate = CreatePredicate(SearchType.GetValueOrDefault(), searchString);
                customers = await customerRepo.Find(predicate);
            }
            else
            {
                customers = await customerRepo.All();
            }

            var customersView = new CustomersViewModel
            {
                Customers = customers.ToList()
            };
            
            
            return View(customersView);

        }
        private Expression<Func<Customer, bool>> CreatePredicate(int SearchType, string searchString)
        {
            Expression<Func<Customer, bool>> newFunc;
            switch (SearchType)
            {
                case 0:
                    newFunc = (c => c.FirstName.Contains(searchString));
                    break;
                case 1:
                    newFunc = (c => c.LastName.Contains(searchString));
                    break;
                case 2:
                    newFunc = (c => c.Email.Contains(searchString));
                    break;
                default:
                    newFunc = null;
                    break;
            }
            return newFunc;
        }

        // GET: Customers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var customer = await _context.Customers
            //    .FirstOrDefaultAsync(m => m.CustomerId == id);
            var customer = await customerRepo.Get(id);

            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // GET: Customers/Create
        [Authorize(Roles = "Admin")]
        public IActionResult CreateNew()
        {
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateNew(Customer customer)
        {
            if (ModelState.IsValid)
            {
                if (customer != null)
                {
                    var cust = await customerRepo.Add(customer);

                    return RedirectToAction("Index", "Customer");
                }
            }
            return View(customer);
        }

        public async Task<IActionResult> OrderHistory(int? id)
        {
            if (id == null)
            {
                var customer = SessionHelper.GetObjectFromJson<Customer>(HttpContext.Session, "Customer");
                if (customer == null)
                {
                    return NotFound();
                }
                id = customer.CustomerId;
            }
            var orders = await orderRepo.Find(o => o.CusomerId == id);
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

        
        public async Task<IActionResult> CustomerOrderHistory()
        {
            var ident = User.FindFirst("id");
            if (ident == null)
            {
                return NotFound();
            }
            var id = int.Parse(ident.Value);

            return RedirectToAction("OrderHistory", id);

            var orders = await orderRepo.Find(o => o.CusomerId == id);

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

        // GET: Customers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await customerRepo.Get(id);//_context.Customers.FindAsync(id);

            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CustomerId,FirstName,LastName,Email")] Customer customer)
        {
            if (id != customer.CustomerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                //_context.Update(customer);
                //await _context.SaveChangesAsync();
                if (null == (customer = await customerRepo.Update(customer)))
                    return NotFound();
                /*
                try
                {
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.CustomerId))
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
            return View(customer);
        }

        // GET: Customers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var customer = await _context.Customers
            //    .FirstOrDefaultAsync(m => m.CustomerId == id);
            var customer = await customerRepo.Get(id);

            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            //var customer = await _context.Customers.FindAsync(id);
            //_context.Customers.Remove(customer);
            //await _context.SaveChangesAsync();
            await customerRepo.Delete(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
