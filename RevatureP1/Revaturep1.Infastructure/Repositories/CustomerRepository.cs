using RevatureP1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Revaturep1.Infastructure.Repositories
{
    public class CustomerRepository : GenericRepository<Customer>
    {
        public CustomerRepository(ShoppingDbContext context) : base(context)
        {
        }

        public override Customer Update(Customer entity)
        {
            var customer = _context.Customers
                .Single(c => c.CustomerId == entity.CustomerId);

            customer.FirstName = entity.FirstName;
            customer.LastName = entity.LastName;
            customer.Email = entity.Email;

            return base.Update(customer);
        }
    }
}
