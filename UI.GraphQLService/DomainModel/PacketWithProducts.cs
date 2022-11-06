using Core.Domain;

namespace UI.GraphQLService.DomainModel
{
    public class PacketWithProducts : Packet
    {
        public new List<Product> Products { get; set; } = new();
        public PacketWithProducts(Packet packet)
            : base(packet.Name, packet.PickUpTimeStart, packet.PickUpTimeEnd, packet.IsAlcoholic, packet.Price, packet.MealTypeId, packet.CanteenId)
        {
        }


    }
}
