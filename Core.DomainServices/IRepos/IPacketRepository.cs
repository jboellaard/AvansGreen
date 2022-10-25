using Core.Domain;

namespace Core.DomainServices.IRepos
{
    public interface IPacketRepository
    {
        IEnumerable<Packet> GetPackets();
        Packet? GetById(int id);
        IEnumerable<Packet> Filter(Func<Packet, bool> filterExpressie);

        IEnumerable<Packet> GetPacketsWithoutReservation();

        IEnumerable<Packet> GetPacketsFromCanteen(int canteen);

        IEnumerable<Packet> GetPacketsReserverdByStudentWithId(int studentId);

        Task AddPacket(Packet packet);

        Task AddProductsToPacket(IEnumerable<PacketProduct> packetProducts);

        Packet? AddReservationToPacket(Packet packet);

    }
}
