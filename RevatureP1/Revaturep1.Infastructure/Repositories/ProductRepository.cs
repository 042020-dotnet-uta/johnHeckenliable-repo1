using Microsoft.EntityFrameworkCore;
using RevatureP1.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Revaturep1.Infastructure.Repositories
{
    public class ProductRepository : GenericRepository<Product>
    {
        public ProductRepository(ShoppingDbContext context) : base(context)
        {
        }

        public override async Task<Product> Update(Product entity)
        {
            var product = await _context.Products
                .SingleAsync(p => p.PoductId == entity.PoductId);

            product.ProductDescription = entity.ProductDescription;
            product.Price = entity.Price;

            return await base.Update(product);
        }
    }
}
