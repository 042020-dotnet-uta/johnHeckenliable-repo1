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
using System.Security.Claims;
using RevatureP1.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace RevatureP1.Web.Controllers
{
    public class CustomersController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CustomersController> _logger;

        public CustomersController(IUnitOfWork unitOfWork, ILogger<CustomersController> logger)
        {
            this._unitOfWork = unitOfWork;
            this._logger = logger;
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
                customers = await _unitOfWork.CustomerRepository.Find(predicate);
            }
            else
            {
                customers = await _unitOfWork.CustomerRepository.All();
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

        public IActionResult ResetSearch()
        {
            return RedirectToAction("Index");
        }

        // GET: Customers/Create
        [Authorize(Roles = "Admin")]
        public IActionResult CreateNew()
        {
            return View();
        }

        public async Task<IActionResult> OrderHistory(int? id)
        {
            if (id == null)
            {
                var customer = SessionHelper.GetObjectFromJson<Customer>(HttpContext.Session, "Customer");
                if (customer == null)
                {
                    _logger.LogDebug("Unable get the customer infor from session in Customers//OrderHistory");
                    return NotFound();
                }
                id = customer.CustomerId;
            }
            var orders = await _unitOfWork.OrderRepository.Find(o => o.CusomerId == id);
            if (orders == null)
            {
                _logger.LogDebug("Unable to locate and orders for customer {0} in Customers//OrderHistory", id);
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
                _logger.LogDebug("The ID was null in Customers//OrderDetails", id);
                return NotFound();
            }
            var order = await _unitOfWork.OrderRepository.Get(id);

            if (order == null)
            {
                _logger.LogDebug("Unable to locate the orders for ID {0} in Customers//OrderHistory", id);
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


        // GET: Customers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                _logger.LogDebug("The ID was null in Customers//Edit");
                return NotFound();
            }

            var customer = await _unitOfWork.CustomerRepository.Get(id);//_context.Customers.FindAsync(id);

            if (customer == null)
            {
                _logger.LogDebug("Unable to locate a customer {0} in Customers//Edit", id);
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
                _logger.LogDebug("The IDs dont match in Customers//Edit(Post)");
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                customer = await _unitOfWork.CustomerRepository.Update(customer);
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }

        // GET: Customers/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                _logger.LogDebug("The ID was null in Customers//Delete");
                return NotFound();
            }

            var customer = await _unitOfWork.CustomerRepository.Get(id);

            if (customer == null)
            {
                _logger.LogDebug("Unable to locate a customer for {0} in Customers//Delete", id);
                return NotFound();
            }

            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            _unitOfWork.CustomerRepository.Delete(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
