using Core.Domain;
using Core.DomainServices.IServices;

namespace Core.DomainServices.Services
{
    public class PacketService : IPacketService
    {
        public Packet CreateNewPacket(string name, DateTime pickUpTimeStart, DateTime pickUpTimeEnd, bool isAlcoholic, decimal price, MealTypeId typeOfMeal, CanteenEmployee canteenEmployee)
        {
            //Packet packet = new Packet();
            throw new NotImplementedException();
        }
    }
}
