using RevatureP1.Models;
using System.Linq;

namespace Revaturep1.Infastructure.Repositories
{
    public class ProductRepository : GenericRepository<Product>
    {
        public ProductRepository(ShoppingDbContext context) : base(context)
        {
        }

        public override Product Update(Product entity)
        {
            var product = _context.Products
                .Single(p => p.PoductId == entity.PoductId);

            product.ProductDescription = entity.ProductDescription;
            product.Price = entity.Price;

            return base.Update(product);
        }
    }
}
