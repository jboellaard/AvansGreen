using Core.Domain;
using UI.AG_PacketsGraphQLAPI.IServices;

namespace UI.AG_PacketsGraphQLAPI.GraphQL
{
    public class PacketQuery
    {
        private readonly IPacketsGraphQLService _packetService;

        public PacketQuery(IPacketsGraphQLService packetService)
        {
            _packetService = packetService;
        }

        [UseFiltering]
        [UseSorting]
        public IQueryable<Packet> packets => _packetService.GetAll();

        [UseFiltering]
        [UseSorting]
        public IQueryable<Packet> availablepackets => _packetService.GetAll().Where(p => !p.StudentId.HasValue && p.PickUpTimeEnd >= DateTime.Now);

        public Packet packet(int id) => _packetService.GetPacketById(id);
    }
}
