using Core.Domain;

namespace Core.DomainServices.IRepos
{
    public interface ICanteenEmployeeRepository
    {
        IEnumerable<CanteenEmployee> GetCanteenWorkers();
        CanteenEmployee? GetById(int id);
        CanteenEmployee? GetByEmail(string email);
    }
}
