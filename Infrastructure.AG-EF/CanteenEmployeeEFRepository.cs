using Core.Domain;
using Core.DomainServices.IRepos;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.AG_EF
{
    public class CanteenEmployeeEFRepository : ICanteenEmployeeRepository
    {
        private readonly AvansGreenDbContext _context;

        public CanteenEmployeeEFRepository(AvansGreenDbContext context)
        {
            _context = context;
        }

        public IEnumerable<CanteenEmployee> GetCanteenWorkers()
        {
            return _context.CanteenEmployees.Include(c => c.Canteen);
        }

        public CanteenEmployee? GetById(int id)
        {
            return _context.CanteenEmployees.Include(c => c.Canteen).SingleOrDefault(canteenWorker => canteenWorker.Id == id);
        }

        public CanteenEmployee? GetByEmployeeNr(string employeeNr)
        {
            return _context.CanteenEmployees.Include(c => c.Canteen).SingleOrDefault(canteenWorker => canteenWorker.EmployeeNr == employeeNr);
        }
    }
}
