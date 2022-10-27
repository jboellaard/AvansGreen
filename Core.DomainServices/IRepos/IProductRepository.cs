using Core.Domain;

namespace Core.DomainServices.IRepos
{
    public interface IProductRepository
    {
        IEnumerable<Product> GetProducts();
        Product? GetById(int id);
    }
}
