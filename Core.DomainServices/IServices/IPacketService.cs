using Core.Domain;

namespace Core.DomainServices.IServices
{
    public interface IPacketService
    {
        Task<Packet> AddPacket(string name,
            int DaysFromNow,
            DateTime pickUpTimeStart,
            DateTime pickUpTimeEnd,
            bool isAlcoholic,
            decimal price,
            MealTypeId typeOfMeal,
            int canteenId,
            List<int> productIdList);

        Packet UpdatePacket(int id,
            string name,
            int DaysFromNow,
            DateTime pickUpTimeStart,
            DateTime pickUpTimeEnd,
            bool isAlcoholic,
            decimal price,
            MealTypeId typeOfMeal,
            int canteenId,
            List<int> productIdList);

        Packet RenewPacket(int id,
            string name,
            int DaysFromNow,
            DateTime pickUpTimeStart,
            DateTime pickUpTimeEnd,
            bool isAlcoholic,
            decimal price,
            MealTypeId typeOfMeal,
            int canteenId,
            List<int> productIdList);

        Packet DeletePacket(int id);

        Packet AddReservation(
            Student student,
            int packetId);

        Packet DeleteReservation(
            Student student,
            int packetId);


    }

}
