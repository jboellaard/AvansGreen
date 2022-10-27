using Core.Domain;

namespace Core.DomainServices.IRepos
{
    public interface ICanteenRepository
    {
        IEnumerable<Canteen> GetCanteens();
        Canteen? GetById(int id);
    }
}
