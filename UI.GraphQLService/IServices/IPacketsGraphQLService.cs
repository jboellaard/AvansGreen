using Core.Domain;

namespace UI.GraphQLService.IServices
{
    public interface IPacketsGraphQLService
    {
        IQueryable<Packet> GetAll();

        Packet GetPacketById(int id);
    }
}
