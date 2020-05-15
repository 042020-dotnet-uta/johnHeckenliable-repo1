using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using RevatureP1.Domain.Utils;
using RevatureP1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revaturep1.DataAccess.Repositories
{
    public class CustomerRepository : GenericRepository<Customer>
    {
        public CustomerRepository(ShoppingDbContext context) : base(context)
        {
        }

        public override async Task<Customer> Add(Customer entity)
        {
            //var hashed = Hash.Sha256(entity.Password);
            //entity.Password = hashed;

            var ent = _context.Add(entity).Entity;
            await _context.SaveChangesAsync();
            return ent;
        }

        public override async Task<Customer> Update(Customer entity)
        {
            var customer = await _context.Customers
                .SingleAsync(c => c.CustomerId == entity.CustomerId);
            if (customer != null)
            {
                customer.FirstName = entity.FirstName;
                customer.LastName = entity.LastName;
                customer.Email = entity.Email;
            }

            return await base.Update(customer);
        }
    }
}
