using Core.Domain;

namespace Core.DomainServices.IRepos
{
    public interface IPacketRepository
    {
        IEnumerable<Packet> GetPackets();
        Packet? GetById(int id);
        IEnumerable<Packet> Filter(Func<Packet, bool> filterExpressie);

        IEnumerable<Packet> GetPacketsCreatedByEmployeeWithId(int CanteenEmployeeId);

        IEnumerable<Packet> GetPacketsReserverdByStudentWithId(int StudentId);

        Task AddPacket(Packet packet);

        Task AddProductsToPacket(IEnumerable<PacketProduct> packetProducts);

    }
}
