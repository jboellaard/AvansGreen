using Core.Domain;
using UI.GraphQLService.IServices;

namespace UI.GraphQLService.GraphQL
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
        public IQueryable<Packet> availablepackets => _packetService.GetAll().Where(p => !p.StudentId.HasValue);

        public Packet packet(int id) => _packetService.GetPacketById(id);
    }
}
