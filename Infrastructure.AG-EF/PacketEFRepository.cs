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
            return _context.Packets.Include(p => p.Student).Include(p => p.Canteen).Include(p => p.Products);
        }

        public Packet? GetById(int id)
        {
            return _context.Packets.Include(p => p.Student).Include(p => p.Canteen).Include(p => p.Products).SingleOrDefault(packet => packet.Id == id);
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

        public Packet? UpdatePacket(Packet packet)
        {
            var productToUpdate = _context.Packets.Include(p => p.Student).FirstOrDefault(p => p.Id == packet.Id);
            if (productToUpdate != null && productToUpdate.Student == null)
            {
                productToUpdate.Name = packet.Name;
                productToUpdate.PickUpTimeStart = packet.PickUpTimeStart;
                productToUpdate.PickUpTimeEnd = packet.PickUpTimeEnd;
                productToUpdate.IsAlcoholic = packet.IsAlcoholic;
                productToUpdate.Price = packet.Price;
                productToUpdate.Products = packet.Products;
                productToUpdate.MealTypeId = packet.MealTypeId;

                _context.SaveChanges();
            }

            return productToUpdate;
        }

        public Packet? DeletePacket(int packetId)
        {
            var packetToRemove = _context.Packets.Include(p => p.Student).FirstOrDefault(p => p.Id == packetId);
            if (packetToRemove != null && packetToRemove.Student == null)
            {
                _context.Packets.Remove(packetToRemove);
                _context.SaveChanges();
            }
            return packetToRemove;
        }

        public async Task AddProductsToPacket(IEnumerable<PacketProduct> packetProducts)
        {
            foreach (PacketProduct packetProduct in packetProducts)
            {
                _context.ProductsInPacket.Add(packetProduct);
            }
            await _context.SaveChangesAsync();
        }

        public IEnumerable<Packet> GetPacketsFromCanteen(int canteenId)
        {
            return _context.Packets.Include(p => p.Canteen).Where(p => p.CanteenId == canteenId);
        }

        public IEnumerable<Packet> GetPacketsReserverdByStudentWithId(int studentId)
        {
            return _context.Packets.Include(p => p.Canteen).Include(p => p.Student).Where(p => p.StudentId == studentId);
        }

        public IEnumerable<Packet> GetPacketsWithoutReservation()
        {
            return _context.Packets.Include(p => p.Student).Where(p => p.Student == null);
        }

        public Packet? AddReservationToPacket(Packet packet)
        {
            var entityToUpdate = _context.Packets.Include(p => p.Student).FirstOrDefault(p => p.Id == packet.Id);
            if (entityToUpdate != null && entityToUpdate.Student == null)
            {
                entityToUpdate.StudentId = packet.StudentId;

                _context.SaveChanges();
            }

            return entityToUpdate;
        }
    }
}
