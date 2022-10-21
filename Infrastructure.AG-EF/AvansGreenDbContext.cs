using Core.Domain;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.AG_EF
{
    public class AvansGreenDbContext : DbContext
    {
        public DbSet<Canteen> Canteens { get; set; }

        public DbSet<CanteenEmployee> CanteenEmployees { get; set; }

        public DbSet<Packet> Packets { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Student> Students { get; set; }

        public DbSet<PacketProduct> ProductsInPacket { get; set; }

        public AvansGreenDbContext(DbContextOptions<AvansGreenDbContext> contextOptions) : base(contextOptions)
        {
            Canteens = Set<Canteen>();
            CanteenEmployees = Set<CanteenEmployee>();
            Packets = Set<Packet>();
            Products = Set<Product>();
            Students = Set<Student>();
            ProductsInPacket = Set<PacketProduct>();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            IEnumerable<Canteen> canteens = new List<Canteen>
            {
                new Canteen { Id = 1, Location = "LA5", City = City.Breda},
                new Canteen { Id = 2, Location = "H1", City = City.Breda },
                new Canteen { Id = 3, Location = "LD1", City = City.Breda },
                new Canteen { Id = 4, Location = "TH1", City = City.Tilburg }
            };

            IEnumerable<CanteenEmployee> canteenEmployees = new List<CanteenEmployee>
            {
                new CanteenEmployee("n.devries@avans.nl") { Id = 1, EmployeeNr = "1234567", CanteenId = canteens.ElementAt(0).Id },
                new CanteenEmployee("p.smit@avans.nl") { Id = 2, EmployeeNr = "1111111", CanteenId = canteens.ElementAt(1).Id },
                new CanteenEmployee("l.degroot@avans.nl") { Id = 3, EmployeeNr = "2222222", CanteenId = canteens.ElementAt(3).Id },
            };

            IEnumerable<Student> students = new List<Student>
            {
                new Student("je.boellaard@student.avans.nl","Joy Boellaard") { Id = 1,  StudentNr = "2182556", DateOfBirth = new DateTime(1998, 11, 11), PhoneNr = "0612345678", CityOfSchool = City.Breda },
                new Student("em.degroot@student.avans.nl", "Emma de Groot") { Id = 2, StudentNr = "2192233", DateOfBirth = new DateTime(2000, 1, 31), PhoneNr = "0623456789", CityOfSchool = City.Breda },
                new Student("b.dejong@student.avans.nl", "Ben de Jong") { Id = 3, StudentNr = "2192344", DateOfBirth = new DateTime(2001, 3, 7), PhoneNr = "0634567890", CityOfSchool = City.Tilburg },
                new Student("d.li@student.avans.nl", "Diana Li") { Id = 4, StudentNr = "2184399", DateOfBirth = new DateTime(1999, 4, 12), PhoneNr = "0645678901", CityOfSchool = City.Breda },
            };

            IEnumerable<Product> products = new List<Product>
            {
                new Product { Id = 1, Name = "Bottle of vodka", IsAlcoholic = true },
                new Product { Id = 2, Name = "Panini", IsAlcoholic = false },
                new Product { Id = 3, Name = "Sandwich", IsAlcoholic = false },
                new Product { Id = 4, Name = "Apple", IsAlcoholic = false },
                new Product { Id = 5, Name = "Soup", IsAlcoholic = false },
                new Product { Id = 6, Name = "Baguette", IsAlcoholic = false }
            };

            IEnumerable<Packet> packets = new List<Packet>
            {
                new Packet { Id = 1, Name = "Alcoholic beverage and snack", TypeOfMeal = MealType.Drink, CanteenId = canteens.ElementAt(0).Id,
                    PickUpTimeStart = new DateTime(2022, 10, 20, 17, 0, 0), PickUpTimeEnd = new DateTime(2022, 10, 20, 20, 0, 0), Price = 5.0, IsAlcoholic = false },
                new Packet { Id = 2, Name = "Lunch with two sandwiches", TypeOfMeal = MealType.Bread, CanteenId = canteens.ElementAt(1).Id,
                    PickUpTimeStart = new DateTime(2022, 10, 21, 13, 0, 0), PickUpTimeEnd = new DateTime(2022, 10, 21, 17, 0, 0), Price = 5.5, IsAlcoholic = false }
            };

            IEnumerable<PacketProduct> productsInPacket = new List<PacketProduct>
            {
                new PacketProduct{ Id = 1, PacketId = packets.ElementAt(0).Id, ProductId = products.ElementAt(0).Id },
                new PacketProduct{ Id = 2, PacketId = packets.ElementAt(0).Id, ProductId = products.ElementAt(3).Id },
                new PacketProduct{ Id = 3, PacketId = packets.ElementAt(1).Id, ProductId = products.ElementAt(2).Id },
            };

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Canteen>().HasData(canteens);
            modelBuilder.Entity<CanteenEmployee>().HasData(canteenEmployees);
            modelBuilder.Entity<Student>().HasData(students);

            modelBuilder.Entity<Product>().HasData(products);
            modelBuilder.Entity<Packet>().HasData(packets);
            modelBuilder.Entity<PacketProduct>().HasData(productsInPacket);

        }
    }
}
