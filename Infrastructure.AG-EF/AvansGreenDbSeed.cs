using Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.AG_EF
{
    public class AvansGreenDbSeed
    {
        private AvansGreenDbContext _context;
        private ILogger<AvansGreenDbSeed> _logger;

        public AvansGreenDbSeed(AvansGreenDbContext context, ILogger<AvansGreenDbSeed> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task EnsurePopulated()
        {
            _context.Database.Migrate();

            if (_context.CanteenEmployees.Count() == 0)
            {
                _logger.LogInformation("Avans green database is being populated.");

                IEnumerable<Canteen> canteens = CanteenEnumerable.GetAll<Canteen>();

                IEnumerable<CanteenEmployee> canteenEmployees = new List<CanteenEmployee>
                {
                    new CanteenEmployee("a0000000", "Admin") { CanteenId = canteens.ElementAt(2).Id },
                    new CanteenEmployee("e1234567", "Naomi de Vries") { CanteenId = canteens.ElementAt(0).Id },
                    new CanteenEmployee("e2345678", "Peter Smit") { CanteenId = canteens.ElementAt(1).Id },
                    new CanteenEmployee("e3456789", "Lennart de Groot") { CanteenId = canteens.ElementAt(3).Id }
                };

                IEnumerable<Student> students = new List<Student>
                {
                    new Student("adminmail@avans.nl", "a0000000", new DateTime(1998, 11, 11), "Admin", "Breda"),
                    new Student("je.boellaard@student.avans.nl", "s2182556", new DateTime(1998, 11, 11), "Joy Boellaard", "Breda"){ PhoneNr = "0612345678" },
                    new Student("em.degroot@student.avans.nl", "s2192233", new DateTime(2006, 1, 31), "Emma de Groot", "Breda"){ PhoneNr = "0623456789", NumberOfTimesNotCollected = 2 },
                    new Student("b.dejong@student.avans.nl", "s2192344", new DateTime(2001, 3, 7), "Ben de Jong", "Tilburg"),
                    new Student("d.li@student.avans.nl", "s2184399", new DateTime(2005, 4, 12), "Diana Li", "Breda"){ PhoneNr = "0645678901" }
                };

                await _context.CanteenEmployees.AddRangeAsync(canteenEmployees);
                await _context.Students.AddRangeAsync(students);
                _context.SaveChanges();

                IEnumerable<Product> products = new List<Product>
                {
                    new Product("Vodka", true){ ProductImageId = 1 },
                    new Product("Panini", false) { ProductImageId = 2 },
                    new Product("Sandwich", false) { ProductImageId = 3 },
                    new Product("Apple", false) { ProductImageId = 4 },
                    new Product("Soup", false) { ProductImageId = 5 },
                    new Product("Baguette", false) { ProductImageId = 6 },
                    new Product("Bacardi", true) {  ProductImageId = 7 },
                    new Product("Beer", true) { ProductImageId = 8 },
                    new Product("Banana", false) { ProductImageId = 9 },
                    new Product("Tangerine", false) { ProductImageId = 10 },
                    new Product("Croquette", false) { ProductImageId = 11 },
                    new Product("Cheese Souffle", false) { ProductImageId = 12 }
                };

                IEnumerable<Packet> packets = new List<Packet>
                {
                    new Packet("Alcoholic beverage and snack", DateTime.Now.Date.AddHours(17), DateTime.Now.Date.AddHours(20), true, 5.0m, MealTypeId.Drink, canteens.ElementAt(0).Id),
                    new Packet("Lunch with two sandwiches", DateTime.Now.Date.AddDays(1).AddHours(13), DateTime.Now.Date.AddDays(1).AddHours(17), false, 5.5m, MealTypeId.Bread, canteens.ElementAt(2).Id),
                    new Packet("Soup and a baguette", DateTime.Now.Date.AddDays(2).AddHours(17), DateTime.Now.Date.AddDays(2).AddHours(19), false, 4.0m, MealTypeId.WarmMeal, canteens.ElementAt(2).Id),
                    new Packet("Fried snacks", DateTime.Now.Date.AddDays(1).AddHours(11), DateTime.Now.Date.AddDays(1).AddHours(15), false, 6.5m, MealTypeId.Snack, canteens.ElementAt(3).Id),
                    new Packet("Left over drinks", DateTime.Now.Date.AddDays(1).AddHours(8), DateTime.Now.Date.AddDays(1).AddHours(12), true, 7m, MealTypeId.Drink, canteens.ElementAt(3).Id),
                    new Packet("Fruit assortment", DateTime.Now.Date.AddDays(1).AddHours(10), DateTime.Now.Date.AddDays(1).AddHours(14), false, 6m, MealTypeId.Drink, canteens.ElementAt(2).Id)
                };

                await _context.Products.AddRangeAsync(products);
                await _context.Packets.AddRangeAsync(packets);
                _context.SaveChanges();

                IEnumerable<PacketProduct> productsInPacket = new List<PacketProduct>
                {
                    new PacketProduct(packets.ElementAt(0).Id, products.ElementAt(0).Id),
                    new PacketProduct(packets.ElementAt(0).Id, products.ElementAt(3).Id),
                    new PacketProduct(packets.ElementAt(1).Id, products.ElementAt(2).Id),
                    new PacketProduct(packets.ElementAt(2).Id, products.ElementAt(5).Id),
                    new PacketProduct(packets.ElementAt(2).Id, products.ElementAt(6).Id),
                    new PacketProduct(packets.ElementAt(3).Id, products.ElementAt(10).Id),
                    new PacketProduct(packets.ElementAt(3).Id, products.ElementAt(11).Id),
                    new PacketProduct(packets.ElementAt(4).Id, products.ElementAt(6).Id),
                    new PacketProduct(packets.ElementAt(4).Id, products.ElementAt(7).Id),
                    new PacketProduct(packets.ElementAt(5).Id, products.ElementAt(3).Id),
                    new PacketProduct(packets.ElementAt(5).Id, products.ElementAt(8).Id),
                    new PacketProduct(packets.ElementAt(5).Id, products.ElementAt(9).Id)
                };
                await _context.ProductsInPacket.AddRangeAsync(productsInPacket);
                _context.SaveChanges();

                _logger.LogInformation("Avans green database has been populated.");

            }
        }
    }
}
