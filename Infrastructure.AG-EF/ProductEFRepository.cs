using Core.Domain;
using Core.DomainServices.IRepos;

namespace Infrastructure.AG_EF
{
    public class ProductEFRepository : IProductRepository
    {
        private readonly AvansGreenDbContext _context;
        public ProductEFRepository(AvansGreenDbContext context)
        {
            _context = context;
        }

        public Product? GetById(int id)
        {
            return _context.Products.SingleOrDefault(product => product.Id == id);
        }

        public IEnumerable<Product> GetProducts()
        {
            return _context.Products;
        }
    }
}
