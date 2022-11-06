using Core.Domain;
using Infrastructure.AG_EF;
using Microsoft.EntityFrameworkCore;
using UI.GraphQLService.IServices;

namespace UI.GraphQLService.Services
{
    public class PacketsGraphQLService : IPacketsGraphQLService
    {
        #region Property  
        private readonly AvansGreenDbContext _context;
        #endregion

        #region Constructor  
        public PacketsGraphQLService(AvansGreenDbContext context)
        {
            _context = context;
        }
        #endregion

        public IQueryable<Packet> GetAll()
        {
            return _context.Packets.AsQueryable().Include(p => p.Products).ThenInclude(pp => pp.Product).Include(p => p.MealType).Include(p => p.Canteen);
        }

        public Packet GetPacketById(int id)
        {
            return _context.Packets.Include(p => p.Products).ThenInclude(pp => pp.Product).Include(p => p.MealType).Include(p => p.Canteen).SingleOrDefault(packet => packet.Id == id);
        }
    }

}

