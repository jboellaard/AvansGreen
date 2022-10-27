using Core.Domain;
using Core.DomainServices.IRepos;

namespace Infrastructure.AG_EF
{
    public class CanteenEFRepository : ICanteenRepository
    {
        private readonly AvansGreenDbContext _context;

        public CanteenEFRepository(AvansGreenDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Canteen> GetCanteens()
        {
            return _context.Canteens;
        }

        public Canteen? GetById(int id)
        {
            return _context.Canteens.SingleOrDefault(canteen => canteen.Id == id);
        }
    }
}
