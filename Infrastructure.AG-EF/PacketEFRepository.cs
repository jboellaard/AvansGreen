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
            return _context.Packets.Include(p => p.Student).Include(p => p.Canteen).Include(p => p.MealType).Include(p => p.Products);
        }

        public Packet? GetById(int id)
        {
            return _context.Packets.Include(p => p.Student).Include(p => p.Canteen).Include(p => p.MealType).Include(p => p.Products).ThenInclude(pp => pp.Product).ThenInclude(pr => pr.ProductImage).SingleOrDefault(packet => packet.Id == id);
        }

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

        public async Task<Packet?> AddPacket(Packet newPacket)
        {
            Packet packet = _context.Packets.Add(newPacket).Entity;
            await _context.SaveChangesAsync();
            return packet;
        }

        public Packet? UpdatePacket(Packet packet)
        {
            var productToUpdate = _context.Packets.Include(p => p.Student).FirstOrDefault(p => p.Id == packet.Id);

            if (productToUpdate != null && (productToUpdate.PickUpTimeEnd < DateTime.Now || (productToUpdate.Student == null && productToUpdate.PickUpTimeEnd >= DateTime.Now)))
            {
                var packetproducts = _context.ProductsInPacket.Where(p => p.PacketId == packet.Id).ToList();
                foreach (PacketProduct product in packetproducts)
                {
                    _context.ProductsInPacket.Remove(product);
                }
                productToUpdate.Name = packet.Name;
                productToUpdate.PickUpTimeStart = packet.PickUpTimeStart;
                productToUpdate.PickUpTimeEnd = packet.PickUpTimeEnd;
                productToUpdate.IsAlcoholic = packet.IsAlcoholic;
                productToUpdate.Price = packet.Price;

                productToUpdate.Products = packet.Products;
                productToUpdate.MealTypeId = packet.MealTypeId;
                productToUpdate.Student = null;
                productToUpdate.StudentId = null;

                _context.SaveChanges();
            }

            return packet;
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
                Product? product = _context.Products.SingleOrDefault(product => product.Id == packetProduct.ProductId);
                if (product.IsAlcoholic)
                {
                    Packet packet = _context.Packets.SingleOrDefault(packet => packet.Id == packetProduct.PacketId);
                    packet.IsAlcoholic = true;
                }
                _context.ProductsInPacket.Add(packetProduct);
            }
            await _context.SaveChangesAsync();
        }

        public IEnumerable<Packet> GetPacketsFromCanteen(int canteenId)
        {
            return _context.Packets.Include(p => p.Canteen).Include(p => p.MealType).Where(p => p.CanteenId == canteenId);
        }

        public IEnumerable<Packet> GetPacketsReserverdByStudentWithId(int studentId)
        {
            return _context.Packets.Include(p => p.Canteen).Include(p => p.MealType).Include(p => p.Student).Where(p => p.StudentId == studentId);
        }

        public IEnumerable<Packet> GetPacketsWithoutReservation()
        {
            return _context.Packets.Include(p => p.Canteen).Include(p => p.MealType).Include(p => p.Student).Where(p => p.Student == null);
        }

        public Packet? AddReservationToPacket(Packet packet)
        {
            var packetToUpdate = _context.Packets.Include(p => p.Student).FirstOrDefault(p => p.Id == packet.Id);
            if (packetToUpdate != null && packetToUpdate.Student == null)
            {
                packetToUpdate.StudentId = packet.StudentId;

                _context.SaveChanges();
                return packetToUpdate;
            }

            return null;
        }

        public Packet? DeleteReservation(int packetId)
        {
            var packetToDeleteReservation = _context.Packets.Include(p => p.Student).FirstOrDefault(p => p.Id == packetId);
            packetToDeleteReservation.Student = null;
            packetToDeleteReservation.StudentId = null;
            _context.SaveChanges();

            return packetToDeleteReservation;
        }
    }
}
