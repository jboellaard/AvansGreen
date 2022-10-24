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
            return _context.Packets.Include(p => p.Student).Include(p => p.CanteenEmployee).ThenInclude(c => c.Canteen).Include(p => p.Products);
        }

        public Packet? GetById(int id)
        {
            return _context.Packets.Include(p => p.Student).Include(p => p.CanteenEmployee).ThenInclude(c => c.Canteen).Include(p => p.Products).SingleOrDefault(packet => packet.Id == id);
        }

        //public void PreLoad()
        //{
        //    var firstPacket = _context.Packets.FirstOrDefault();

        //    if (firstPacket != null)
        //    {
        //        _context.Entry(firstPacket).Collection(packet => packet.Products).Load();
        //        _context.Entry(firstPacket).Reference(packet => packet.ReservedByStudent).Load();
        //    }
        //}

        public IEnumerable<Packet> Filter(Func<Packet, bool> filterExpression)
        {
            foreach (var packet in _context.Packets)
            {
                if (filterExpression(packet))
                {
                    yield return packet;
                }
            }
        }

        public async Task AddPacket(Packet newPacket)
        {
            _context.Packets.Add(newPacket);
            await _context.SaveChangesAsync();
        }

        public async Task AddProductsToPacket(IEnumerable<PacketProduct> packetProducts)
        {
            foreach (PacketProduct packetProduct in packetProducts)
            {
                _context.ProductsInPacket.Add(packetProduct);
            }
            await _context.SaveChangesAsync();
        }

        public IEnumerable<Packet> GetPacketsCreatedByEmployeeWithId(int CanteenEmployeeId)
        {
            return _context.Packets.Where(p => p.CanteenEmployeeId == CanteenEmployeeId);
        }

        public IEnumerable<Packet> GetPacketsReserverdByStudentWithId(int StudentId)
        {
            return _context.Packets.Where(p => p.StudentId == StudentId);
        }
    }
}
