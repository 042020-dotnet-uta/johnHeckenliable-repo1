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

namespace RevatureP1.Web.Controllers
{
    public class CustomersRepository : Controller
    {
        private readonly CustomerRepository customerRepo;

        public CustomersRepository(CustomerRepository customerRepo)
        {
            this.customerRepo = customerRepo;
        }

        // GET: Customers
        public async Task<IActionResult> Index()
        {
            //return View(await _context.Customers.ToListAsync());
            return View(await customerRepo.All());
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
        public IActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CustomerId,FirstName,LastName,Email")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                //_context.Add(customer);
                //await _context.SaveChangesAsync();
                customer = await customerRepo.Add(customer);

                return RedirectToAction(nameof(Index));
            }
            return View(customer);
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
            var customer =await  customerRepo.Get(id);

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
