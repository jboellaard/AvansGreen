using Core.Domain;

namespace Core.DomainServices.IRepos
{
    public interface IPacketRepository
    {
        IEnumerable<Packet> GetPackets();
        IEnumerable<Packet> Filter(Func<Packet, bool> filterExpressie);
        Packet? GetById(int id);
        Task AddPacket(Packet packet);

        Task AddProductsToPacket(IEnumerable<PacketProduct> packetProducts);
    }
}
