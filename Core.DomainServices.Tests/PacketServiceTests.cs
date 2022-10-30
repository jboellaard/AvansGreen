using Core.Domain;
using Core.DomainServices.IRepos;
using Core.DomainServices.Services;
using Moq;
using System.Diagnostics;

namespace Core.DomainServices.Tests
{
    public class PacketServiceTests
    {
        string name;
        int daysFromNow;
        DateTime pickUpTimeStart;
        DateTime pickUpTimeEnd;
        bool isAlcoholic;
        decimal price;
        MealTypeId typeOfMeal;
        int canteenId;
        List<int> productIdList;

        Mock<IPacketRepository> packetRepoMock;
        Mock<IProductRepository> productRepoMock;
        Mock<IStudentRepository> studentRepoMock;
        Mock<ICanteenEmployeeRepository> canteenEmployeeRepoMock;

        public PacketServiceTests()
        {
            name = "name";
            daysFromNow = 1;
            pickUpTimeStart = DateTime.Now.Date.AddHours(16);
            pickUpTimeEnd = DateTime.Now.Date.AddHours(19);
            isAlcoholic = false;
            price = 5.0m;
            typeOfMeal = MealTypeId.Bread;
            canteenId = 1;
            productIdList = new List<int>();

            packetRepoMock = new Mock<IPacketRepository>();
            productRepoMock = new Mock<IProductRepository>();
            studentRepoMock = new Mock<IStudentRepository>();
            canteenEmployeeRepoMock = new Mock<ICanteenEmployeeRepository>();
        }

        [Fact]
        public async void PickUpTimeEnd_Before_Now_Throws_Exception()
        {
            //Arrange
            daysFromNow = -1;

            Packet packet = new Packet(name, pickUpTimeStart.AddDays(daysFromNow), pickUpTimeEnd.AddDays(daysFromNow), isAlcoholic, price, typeOfMeal, canteenId);
            packetRepoMock.Setup(x => x.AddPacket(packet)).ReturnsAsync(packet);

            bool exceptionThrown = false;
            string message = "";

            PacketService packetService = new(packetRepoMock.Object, productRepoMock.Object, studentRepoMock.Object, canteenEmployeeRepoMock.Object);

            //Act
            try
            {
                Packet? createdPacket = await packetService.AddPacket(name, daysFromNow, pickUpTimeStart, pickUpTimeEnd, isAlcoholic, price, typeOfMeal, canteenId, productIdList);
            }
            catch (InvalidOperationException e)
            {
                exceptionThrown = true;
                message = e.Message;
            }

            //Assert
            Assert.True(exceptionThrown);
            Assert.Equal("Pick up time cannot be in the past.", message);
        }

        [Fact]
        public void Trying_To_Edit_Packet_With_Reservation_Throws_Exception()
        {
            // Arrange
            Packet packet = new(name, pickUpTimeStart.AddDays(daysFromNow), pickUpTimeEnd.AddDays(daysFromNow), isAlcoholic, price, typeOfMeal, canteenId)
            {
                Id = 1,
                StudentId = 1
            };
            packetRepoMock.Setup(x => x.GetById(packet.Id)).Returns(packet);
            packetRepoMock.Setup(x => x.DeleteReservation(packet.Id)).Returns(packet);

            bool exceptionThrown = false;
            string message = "";

            PacketService packetService = new PacketService(packetRepoMock.Object, productRepoMock.Object, studentRepoMock.Object, canteenEmployeeRepoMock.Object);

            //Act
            try
            {
                Packet? updatedPacket = packetService.UpdatePacket(packet.Id, name, daysFromNow, pickUpTimeStart, pickUpTimeEnd, isAlcoholic, price, typeOfMeal, canteenId, productIdList);
            }
            catch (InvalidOperationException e)
            {
                exceptionThrown = true;
                message = e.Message;
            }

            //Assert
            Assert.True(exceptionThrown);
            Assert.Equal("This packet has a reservation and cannot be edited.", message);
        }

        [Fact]
        public void Trying_To_Delete_Packet_With_Reservation_Throws_Exception()
        {
            // Arrange
            Packet packet = new(name, pickUpTimeStart.AddDays(daysFromNow), pickUpTimeEnd.AddDays(daysFromNow), isAlcoholic, price, typeOfMeal, canteenId)
            {
                Id = 1,
                StudentId = 1
            };
            packetRepoMock.Setup(x => x.GetById(packet.Id)).Returns(packet);
            packetRepoMock.Setup(x => x.DeleteReservation(packet.Id)).Returns(packet);

            bool exceptionThrown = false;
            string message = "";

            PacketService packetService = new PacketService(packetRepoMock.Object, productRepoMock.Object, studentRepoMock.Object, canteenEmployeeRepoMock.Object);

            //Act
            try
            {
                Packet? deletedPacket = packetService.DeletePacket(packet.Id);
            }
            catch (InvalidOperationException e)
            {
                exceptionThrown = true;
                message = e.Message;
            }

            //Assert
            Assert.True(exceptionThrown);
            Assert.Equal("This packet has a reservation and cannot be removed.", message);
        }

        [Fact]
        public void Trying_To_Reserve_Packet_With_Reservation_Throws_Exception()
        {
            // Arrange
            Packet packet = new(name, pickUpTimeStart.AddDays(daysFromNow), pickUpTimeEnd.AddDays(daysFromNow), isAlcoholic, price, typeOfMeal, canteenId)
            {
                Id = 1,
                StudentId = 1
            };
            Student student = new Student("email@address.com", "s0000000", DateTime.Now.AddYears(-18), "Name", "Breda") { Id = 2 };
            packetRepoMock.Setup(x => x.GetById(packet.Id)).Returns(packet);
            packetRepoMock.Setup(x => x.AddReservationToPacket(packet)).Returns(packet);

            bool exceptionThrown = false;
            string message = "";

            PacketService packetService = new(packetRepoMock.Object, productRepoMock.Object, studentRepoMock.Object, canteenEmployeeRepoMock.Object);

            //Act
            try
            {
                Packet? packetWithReservation = packetService.AddReservation(student, packet.Id);
            }
            catch (InvalidOperationException e)
            {
                exceptionThrown = true;
                message = e.Message;
            }

            //Assert
            Assert.True(exceptionThrown);
            Assert.Equal("This packet has already been reserved.", message);
        }

        [Fact]
        public void Student_Cannot_Make_More_Than_One_Reservation_Per_Day()
        {
            // Arrange
            Packet packet = new(name, pickUpTimeStart.AddDays(daysFromNow), pickUpTimeEnd.AddDays(daysFromNow), isAlcoholic, price, typeOfMeal, canteenId)
            {
                Id = 1,
                StudentId = 1
            };
            Student student = new("email@address.com", "s0000000", DateTime.Now.AddYears(-18), "Name", "Breda") { Id = 1, ReservedPackets = new List<Packet> { packet } };
            Packet newPacket = new(name, pickUpTimeStart.AddDays(daysFromNow), pickUpTimeEnd.AddDays(daysFromNow), isAlcoholic, price, typeOfMeal, canteenId)
            {
                Id = 2,
            };
            packetRepoMock.Setup(x => x.GetById(newPacket.Id)).Returns(newPacket);
            packetRepoMock.Setup(x => x.AddReservationToPacket(newPacket)).Returns(newPacket);

            bool exceptionThrown = false;
            string message = "";

            PacketService packetService = new(packetRepoMock.Object, productRepoMock.Object, studentRepoMock.Object, canteenEmployeeRepoMock.Object);

            //Act
            try
            {
                Packet? packetWithReservation = packetService.AddReservation(student, newPacket.Id);
            }
            catch (InvalidOperationException e)
            {
                exceptionThrown = true;
                message = e.Message;
            }

            //Assert
            Assert.True(exceptionThrown);
            Assert.Equal("You cannot make more than one reservation per day.", message);
        }

        [Fact]
        public void Trying_To_Reserve_Alcoholic_Packet_When_Under_18_Throws_Exception()
        {
            // Arrange
            isAlcoholic = true;

            Packet packet = new(name, pickUpTimeStart.AddDays(daysFromNow), pickUpTimeEnd.AddDays(daysFromNow), isAlcoholic, price, typeOfMeal, canteenId)
            {
                Id = 1
            };
            Student student = new Student("email@address.com", "s0000000", DateTime.Now.AddYears(-17), "Name", "Breda") { Id = 1 };
            packetRepoMock.Setup(x => x.GetById(packet.Id)).Returns(packet);
            packetRepoMock.Setup(x => x.AddReservationToPacket(packet)).Returns(packet);

            bool exceptionThrown = false;
            string message = "";

            PacketService packetService = new(packetRepoMock.Object, productRepoMock.Object, studentRepoMock.Object, canteenEmployeeRepoMock.Object);

            //Act
            try
            {
                Packet? packetWithReservation = packetService.AddReservation(student, packet.Id);
            }
            catch (InvalidOperationException e)
            {
                exceptionThrown = true;
                message = e.Message;
            }

            //Assert
            Assert.True(exceptionThrown);
            Assert.Equal("You are too young to reserve a packet with alcohol.", message);
        }


        [Fact]
        public void If_Student_Is_Allowed_To_Reserve_Packet_Reservation_Is_Added()
        {
            // Arrange
            Packet packet = new(name, pickUpTimeStart.AddDays(daysFromNow), pickUpTimeEnd.AddDays(daysFromNow), isAlcoholic, price, typeOfMeal, canteenId)
            {
                Id = 1
            };
            Student student = new Student("email@address.com", "s0000000", DateTime.Now.AddYears(-18), "Name", "Breda") { Id = 1 };
            packetRepoMock.Setup(x => x.GetById(packet.Id)).Returns(packet);
            packetRepoMock.Setup(x => x.AddReservationToPacket(packet)).Returns(packet);

            bool exceptionThrown = false;

            PacketService packetService = new(packetRepoMock.Object, productRepoMock.Object, studentRepoMock.Object, canteenEmployeeRepoMock.Object);

            //Act
            Packet? packetWithReservation = null;
            try
            {
                packetWithReservation = packetService.AddReservation(student, packet.Id);
            }
            catch (InvalidOperationException e)
            {
                exceptionThrown = true;
            }

            //Assert
            Assert.False(exceptionThrown);
            Assert.Equal(packet, packetWithReservation);
        }

        [Fact]
        public async void Adding_Alcoholic_Product_To_New_Packet_Makes_Packet_Alcoholic()
        {
            //Arrange
            productIdList = new() { 1 };

            Packet packet = new(name, pickUpTimeStart.AddDays(daysFromNow), pickUpTimeEnd.AddDays(daysFromNow), isAlcoholic, price, typeOfMeal, canteenId);
            Product alcoholicProduct = new("name", true) { Id = 1 };
            packetRepoMock.Setup(x => x.GetById(packet.Id)).Returns(packet);
            packetRepoMock.Setup(x => x.AddPacket(It.IsAny<Packet>())).ReturnsAsync(packet);
            productRepoMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(alcoholicProduct);

            bool exceptionThrown = false;

            PacketService packetService = new(packetRepoMock.Object, productRepoMock.Object, studentRepoMock.Object, canteenEmployeeRepoMock.Object);
            Packet? createdPacket = null;
            //Act
            try
            {
                createdPacket = await packetService.AddPacket(name, daysFromNow, pickUpTimeStart, pickUpTimeEnd, isAlcoholic, price, typeOfMeal, canteenId, productIdList);
            }
            catch (InvalidOperationException e)
            {
                exceptionThrown = true;
            }

            //Assert
            Assert.False(exceptionThrown);
            Assert.True(createdPacket.IsAlcoholic);
        }

        [Fact]
        public void Adding_Alcoholic_Product_To_Existing_Packet_Makes_Packet_Alcoholic()
        {
            //Arrange
            productIdList = new() { 1 };

            Packet packet = new(name, pickUpTimeStart.AddDays(daysFromNow), pickUpTimeEnd.AddDays(daysFromNow), isAlcoholic, price, typeOfMeal, canteenId)
            {
                Id = 1
            };
            Packet packetWithAlcohol = new(name, pickUpTimeStart.AddDays(daysFromNow), pickUpTimeEnd.AddDays(daysFromNow), true, price, typeOfMeal, canteenId);
            Product alcoholicProduct = new("name", true) { Id = 1 };
            packetRepoMock.Setup(x => x.GetById(packet.Id)).Returns(packet);
            packetRepoMock.Setup(x => x.UpdatePacket(It.Is<Packet>(p => p.IsAlcoholic))).Returns(packetWithAlcohol);
            productRepoMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(alcoholicProduct);

            bool exceptionThrown = false;

            PacketService packetService = new(packetRepoMock.Object, productRepoMock.Object, studentRepoMock.Object, canteenEmployeeRepoMock.Object);
            Packet? updatedPacket = null;
            //Act
            try
            {
                updatedPacket = packetService.UpdatePacket(packet.Id, name, daysFromNow, pickUpTimeStart, pickUpTimeEnd, isAlcoholic, price, typeOfMeal, canteenId, productIdList);
            }
            catch (InvalidOperationException e)
            {
                exceptionThrown = true;
            }

            //Assert
            Assert.False(exceptionThrown);
            Assert.True(updatedPacket.IsAlcoholic);
        }

        [Fact]
        public async void Warm_Meal_Cannot_Be_Added_If_Location_Does_Not_Offer_Warm_Meals()
        {
            //Arrange
            typeOfMeal = MealTypeId.WarmMeal;
            canteenId = CanteenEnumerable.FromName<Canteen>("TH1").Id;

            Packet packet = new(name, pickUpTimeStart.AddDays(daysFromNow), pickUpTimeEnd.AddDays(daysFromNow), isAlcoholic, price, typeOfMeal, canteenId);
            packetRepoMock.Setup(x => x.AddPacket(packet)).ReturnsAsync(packet);

            bool exceptionThrown = false;
            string message = "";

            PacketService packetService = new(packetRepoMock.Object, productRepoMock.Object, studentRepoMock.Object, canteenEmployeeRepoMock.Object);

            //Act
            try
            {
                Packet? createdPacket = await packetService.AddPacket(name, daysFromNow, pickUpTimeStart, pickUpTimeEnd, isAlcoholic, price, typeOfMeal, canteenId, productIdList);
            }
            catch (InvalidOperationException e)
            {
                exceptionThrown = true;
                message = e.Message;
            }

            //Assert
            Assert.True(exceptionThrown);
            Assert.Equal("You cannot add warm meals at this location, please choose a different type of meal.", message);
        }

        [Fact]
        public async void Warm_Meal_Can_Be_Added_If_Location_Offers_Warm_Meals()
        {
            //Arrange
            typeOfMeal = MealTypeId.WarmMeal;
            canteenId = CanteenEnumerable.FromName<Canteen>("DH1").Id;

            Packet packet = new(name, pickUpTimeStart.AddDays(daysFromNow), pickUpTimeEnd.AddDays(daysFromNow), isAlcoholic, price, typeOfMeal, canteenId);
            packetRepoMock.Setup(x => x.AddPacket(It.IsAny<Packet>())).ReturnsAsync(packet);

            bool exceptionThrown = false;

            PacketService packetService = new(packetRepoMock.Object, productRepoMock.Object, studentRepoMock.Object, canteenEmployeeRepoMock.Object);

            Packet? createdPacket = null;
            //Act
            try
            {
                createdPacket = await packetService.AddPacket(name, daysFromNow, pickUpTimeStart, pickUpTimeEnd, isAlcoholic, price, typeOfMeal, canteenId, productIdList);
            }
            catch (InvalidOperationException e)
            {
                exceptionThrown = true;
                Trace.Write(e.Message);
            }

            //Assert
            Assert.False(exceptionThrown);
            Assert.Equal(packet, createdPacket);
        }

        [Fact]
        public void Packet_Updates_If_Edit_Is_Valid()
        {
            // Arrange
            Packet packet = new(name, pickUpTimeStart.AddDays(daysFromNow), pickUpTimeEnd.AddDays(daysFromNow), isAlcoholic, price, typeOfMeal, canteenId)
            {
                Id = 1
            };
            packetRepoMock.Setup(x => x.GetById(packet.Id)).Returns(packet);
            packetRepoMock.Setup(x => x.UpdatePacket(It.IsAny<Packet>())).Returns(packet);

            bool exceptionThrown = false;

            PacketService packetService = new PacketService(packetRepoMock.Object, productRepoMock.Object, studentRepoMock.Object, canteenEmployeeRepoMock.Object);
            Packet? updatedPacket = null;
            //Act
            try
            {
                updatedPacket = packetService.UpdatePacket(packet.Id, name, daysFromNow, pickUpTimeStart, pickUpTimeEnd, isAlcoholic, price, typeOfMeal, canteenId, productIdList);
            }
            catch (InvalidOperationException e)
            {
                exceptionThrown = true;
            }

            //Assert
            Assert.False(exceptionThrown);
            Assert.Equal(packet, updatedPacket);
        }

        [Fact]
        public void Packet_Deletes_If_Allowed()
        {
            // Arrange
            Packet packet = new(name, pickUpTimeStart.AddDays(daysFromNow), pickUpTimeEnd.AddDays(daysFromNow), isAlcoholic, price, typeOfMeal, canteenId)
            {
                Id = 1
            };
            packetRepoMock.Setup(x => x.GetById(packet.Id)).Returns(packet);
            packetRepoMock.Setup(x => x.DeletePacket(packet.Id)).Returns(packet);

            bool exceptionThrown = false;

            PacketService packetService = new PacketService(packetRepoMock.Object, productRepoMock.Object, studentRepoMock.Object, canteenEmployeeRepoMock.Object);
            Packet? deletedPacket = null;
            //Act
            try
            {
                deletedPacket = packetService.DeletePacket(packet.Id);
            }
            catch (InvalidOperationException e)
            {
                exceptionThrown = true;
            }

            //Assert
            Assert.False(exceptionThrown);
            Assert.Equal(packet, deletedPacket);
        }

    }
}