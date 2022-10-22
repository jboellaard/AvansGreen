using Core.Domain;

namespace Core.DomainServices.IServices
{
    public interface IPacketService
    {
        Packet CreateNewPacket(
            string name,
            DateTime pickUpTimeStart,
            DateTime pickUpTimeEnd,
            bool isAlcoholic,
            decimal price,
            MealType typeOfMeal,
            CanteenEmployee canteenEmployee);
    }
}
