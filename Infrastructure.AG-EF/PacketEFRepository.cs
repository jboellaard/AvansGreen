using Core.Domain;
using Core.DomainServices.IRepos;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.AG_EF
{
    public class PacketEFRepository : IPacketRepository
    {

        private readonly AvansGreenDbContext _context;

        public PacketEFRepository(AvansGreenDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Packet> GetPackets()
        {
            return _context.Packets;
        }

        public Packet? GetById(int id)
        {
            return _context.Packets.SingleOrDefault(packet => packet.Id == id);
        }

        public void PreLoad()
        {
            var firstPacket = _context.Packets.FirstOrDefault();

            if (firstPacket != null)
            {
                _context.Entry(firstPacket).Collection(packet => packet.Products).Load();
                _context.Entry(firstPacket).Reference(packet => packet.ReservedBy).Load();
            }
        }

        public IQueryable<Packet> GetAll()
        {
            return _context.Packets.Include(p => p.ReservedBy).Include(p => p.Products).Include(p => p.Canteen);
        }

        public IEnumerable<Packet> Filter(Func<Packet, bool> filterExpression)
        {
            foreach (var game in _context.Packets)
            {
                if (filterExpression(game))
                {
                    yield return game;
                }
            }
        }

        public async Task AddPacket(Packet newPacket)
        {
            _context.Packets.Add(newPacket);
            await _context.SaveChangesAsync();
        }
    }
}
