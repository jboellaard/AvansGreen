using Core.Domain;

namespace UI.AG_PacketsGraphQLAPI.IServices
{
    public interface IPacketsGraphQLService
    {
        IQueryable<Packet> GetAll();

        Packet GetPacketById(int id);
    }
}
