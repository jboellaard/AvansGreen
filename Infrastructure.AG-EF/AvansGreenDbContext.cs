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
                new Canteen("LA5", City.Breda) { Id = 1 },
                new Canteen("H1", City.Breda) { Id = 2 },
                new Canteen("LD1", City.Breda) { Id = 3 },
                new Canteen("TH1", City.Tilburg) { Id = 4 }
            };

            IEnumerable<CanteenEmployee> canteenEmployees = new List<CanteenEmployee>
            {
                new CanteenEmployee("0000000", "adminmail@avans.nl") { Id = 1, CanteenId = canteens.ElementAt(2).Id },
                new CanteenEmployee("1234567", "n.devries@avans.nl") { Id = 2, CanteenId = canteens.ElementAt(0).Id },
                new CanteenEmployee("1234567", "p.smit@avans.nl") { Id = 3, CanteenId = canteens.ElementAt(1).Id },
                new CanteenEmployee("1234567", "l.degroot@avans.nl") { Id = 4, CanteenId = canteens.ElementAt(3).Id }
            };

            IEnumerable<Student> students = new List<Student>
            {
                new Student("adminmail@avans.nl", "0000000", new DateTime(1998, 11, 11), "Admin", City.Breda) { Id = 1 },
                new Student("je.boellaard@student.avans.nl", "2182556", new DateTime(1998, 11, 11), "Joy Boellaard", City.Breda){ Id = 2, PhoneNr = "0612345678" },
                new Student("em.degroot@student.avans.nl", "2192233", new DateTime(2000, 1, 31), "Emma de Groot", City.Breda){ Id = 3, PhoneNr = "0623456789" },
                new Student("b.dejong@student.avans.nl", "2192344", new DateTime(2001, 3, 7), "Ben de Jong", City.Tilburg){ Id = 4 },
                new Student("d.li@student.avans.nl", "2184399", new DateTime(1999, 4, 12), "Diana Li", City.Breda){ Id = 5, PhoneNr = "0645678901" }
            };

            IEnumerable<Product> products = new List<Product>
            {
                new Product("Bottle of vodka"){ Id = 1, IsAlcoholic = true },
                new Product("Panini") { Id = 2, IsAlcoholic = false },
                new Product("Sandwich") { Id = 3, IsAlcoholic = false },
                new Product("Apple") { Id = 4, IsAlcoholic = false },
                new Product("Soup") { Id = 5, IsAlcoholic = false },
                new Product("Baguette") { Id = 6, IsAlcoholic = false }
            };

            IEnumerable<Packet> packets = new List<Packet>
            {
                new Packet("Alcoholic beverage and snack", new DateTime(2022, 10, 20, 17, 0, 0), new DateTime(2022, 10, 20, 20, 0, 0), true, 5.0m, MealType.Drink, canteenEmployees.ElementAt(2).Id) { Id = 1, IsAlcoholic = false },
                new Packet("Lunch with two sandwiches", new DateTime(2022, 10, 21, 13, 0, 0), new DateTime(2022, 10, 21, 17, 0, 0), false, 5.5m, MealType.Bread, canteenEmployees.ElementAt(1).Id) { Id = 2, IsAlcoholic = false }
            };

            IEnumerable<PacketProduct> productsInPacket = new List<PacketProduct>
            {
                new PacketProduct(packets.ElementAt(0).Id, products.ElementAt(0).Id){ Id = 1 },
                new PacketProduct(packets.ElementAt(0).Id, products.ElementAt(3).Id){ Id = 2 },
                new PacketProduct(packets.ElementAt(1).Id, products.ElementAt(2).Id){ Id = 3 },
            };

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Canteen>().HasData(canteens);
            modelBuilder.Entity<CanteenEmployee>().HasData(canteenEmployees);
            modelBuilder.Entity<Student>().HasData(students);

            modelBuilder.Entity<Product>().HasData(products);
            modelBuilder.Entity<Packet>().HasData(packets);
            modelBuilder.Entity<PacketProduct>().HasData(productsInPacket);

            modelBuilder.Entity<Student>(entity =>
            {
                entity.HasIndex(e => e.EmailAddress).IsUnique();
                entity.HasIndex(e => e.StudentNr).IsUnique();
            });

            modelBuilder.Entity<CanteenEmployee>(entity =>
            {
                entity.HasIndex(e => e.EmailAddress).IsUnique();
            });

            modelBuilder.Entity<Packet>().Property(p => p.Price).HasPrecision(6, 2);

        }
    }
}
