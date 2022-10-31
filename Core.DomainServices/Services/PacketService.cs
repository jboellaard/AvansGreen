using Core.Domain;
using Core.DomainServices.IRepos;
using Core.DomainServices.IServices;

namespace Core.DomainServices.Services
{
    public class PacketService : IPacketService
    {
        private readonly IPacketRepository _packetRepository;
        private readonly IProductRepository _productRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly ICanteenEmployeeRepository _canteenEmployeeRepository;

        public PacketService(IPacketRepository packetRepository,
            IProductRepository productRepository,
            IStudentRepository studentRepository,
            ICanteenEmployeeRepository canteenEmployeeRepository
            )
        {
            _packetRepository = packetRepository;
            _productRepository = productRepository;
            _studentRepository = studentRepository;
            _canteenEmployeeRepository = canteenEmployeeRepository;
        }

        public async Task<Packet> AddPacket(string name, int daysFromNow, DateTime pickUpTimeStart, DateTime pickUpTimeEnd, bool isAlcoholic, decimal price, MealTypeId typeOfMeal, int canteenId, List<int> productIdList)
        {
            Canteen canteen = CanteenEnumerable.FromId<Canteen>(canteenId);
            if (!canteen.HasWarmMeals && typeOfMeal.Equals(MealTypeId.WarmMeal))
            {
                throw new InvalidOperationException("You cannot add warm meals at this location, please choose a different type of meal.");
            }

            pickUpTimeStart = pickUpTimeStart.AddDays(daysFromNow);
            pickUpTimeEnd = pickUpTimeEnd.AddDays(daysFromNow);
            if (pickUpTimeEnd < DateTime.Now)
            {
                throw new InvalidOperationException("Pick up time cannot be in the past.");
            }

            Packet newPacket = new(name, pickUpTimeStart, pickUpTimeEnd, isAlcoholic, price, typeOfMeal, canteenId);
            Packet createdPacket = await _packetRepository.AddPacket(newPacket);

            foreach (int id in productIdList)
            {
                Product product = _productRepository.GetById(id);
                if (product.IsAlcoholic)
                {
                    createdPacket.IsAlcoholic = true;
                }
                createdPacket.Products.Add(new PacketProduct(createdPacket.Id, id));
            }
            await _packetRepository.AddProductsToPacket(createdPacket.Products);
            return createdPacket;
        }

        public Packet UpdatePacket(int id, string name, int daysFromNow, DateTime pickUpTimeStart, DateTime pickUpTimeEnd, bool isAlcoholic, decimal price, MealTypeId typeOfMeal, int canteenId, List<int> productIdList)
        {
            Packet packet = _packetRepository.GetById(id);
            if (packet.StudentId.HasValue)
            {
                throw new InvalidOperationException("This packet has a reservation and cannot be edited.");
            }
            return RenewPacket(id, name, daysFromNow, pickUpTimeStart, pickUpTimeEnd, isAlcoholic, price, typeOfMeal, canteenId, productIdList);
        }

        public Packet RenewPacket(int id, string name, int daysFromNow, DateTime pickUpTimeStart, DateTime pickUpTimeEnd, bool isAlcoholic, decimal price, MealTypeId typeOfMeal, int canteenId, List<int> productIdList)
        {
            Canteen canteen = CanteenEnumerable.FromId<Canteen>(canteenId);
            if (!canteen.HasWarmMeals && typeOfMeal.Equals(MealTypeId.WarmMeal))
            {
                throw new InvalidOperationException("You cannot add warm meals at this location, please choose a different type of meal.");
            }

            pickUpTimeStart = pickUpTimeStart.AddDays(daysFromNow);
            pickUpTimeEnd = pickUpTimeEnd.AddDays(daysFromNow);
            if (pickUpTimeEnd < DateTime.Now)
            {
                throw new InvalidOperationException("Pick up time cannot be in the past.");
            }

            Packet newPacket = new(name, pickUpTimeStart, pickUpTimeEnd, isAlcoholic, price, typeOfMeal, canteenId) { Id = id };
            foreach (int productId in productIdList)
            {
                Product product = _productRepository.GetById(productId);
                if (product.IsAlcoholic)
                {
                    newPacket.IsAlcoholic = true;
                }
                newPacket.Products.Add(new PacketProduct(newPacket.Id, productId));
            }
            Packet updatedPacket = _packetRepository.UpdatePacket(newPacket);
            return updatedPacket;
        }

        public Packet DeletePacket(int id)
        {
            Packet packet = _packetRepository.GetById(id)!;
            if (packet.StudentId.HasValue && packet.PickUpTimeEnd >= DateTime.Now)
            {
                throw new InvalidOperationException("This packet has a reservation and cannot be removed.");
            }
            else
            {
                return _packetRepository.DeletePacket(packet.Id);
            }
        }

        public Packet AddReservation(Student student, int packetId)
        {
            if (student != null)
            {
                Packet packet = _packetRepository.GetById(packetId)!;
                if (packet.StudentId.HasValue)
                {
                    throw new InvalidOperationException("This packet has already been reserved.");
                }
                if (!(packet.PickUpTimeEnd.Date < student.DateOfBirth.AddYears(18).Date && packet.IsAlcoholic))
                {
                    foreach (Packet reservedPacket in student.ReservedPackets)
                    {
                        if (reservedPacket.PickUpTimeStart.Date.Equals(packet.PickUpTimeStart.Date))
                        {
                            throw new InvalidOperationException("You cannot make more than one reservation per day.");
                        }
                    }
                }
                else
                {
                    throw new InvalidOperationException("You are too young to reserve a packet with alcohol.");
                }
                packet.StudentId = student.Id;
                Packet updatedPacket = _packetRepository.AddReservationToPacket(packet);
                if (updatedPacket != null) return updatedPacket;
            }
            throw new InvalidOperationException("Could not add reservation, please try again later.");
        }

        public Packet DeleteReservation(Student student, int packetId)
        {
            if (student != null)
            {
                Packet packet = _packetRepository.DeleteReservation(packetId);
                if (packet != null)
                {
                    return packet;
                }
            }
            throw new InvalidOperationException("Reservation could not be cancelled, please try again later.");
        }
    }
}
