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

        public DbSet<ProductImage> ProductImages { get; set; }

        public DbSet<Student> Students { get; set; }

        public DbSet<PacketProduct> ProductsInPacket { get; set; }

        public DbSet<MealType> MealTypes { get; set; }


        public AvansGreenDbContext(DbContextOptions<AvansGreenDbContext> contextOptions) : base(contextOptions)
        {
            Canteens = Set<Canteen>();
            CanteenEmployees = Set<CanteenEmployee>();
            Packets = Set<Packet>();
            Products = Set<Product>();
            ProductImages = Set<ProductImage>();
            Students = Set<Student>();
            ProductsInPacket = Set<PacketProduct>();
            MealTypes = Set<MealType>();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            IEnumerable<Canteen> canteens = CanteenEnumerable.GetAll<Canteen>();

            IEnumerable<CanteenEmployee> canteenEmployees = new List<CanteenEmployee>
            {
                new CanteenEmployee("a0000000", "Admin") { Id = 1, CanteenId = canteens.ElementAt(2).Id },
                new CanteenEmployee("e1234567", "Naomi de Vries") { Id = 2, CanteenId = canteens.ElementAt(0).Id },
                new CanteenEmployee("e2345678", "Peter Smit") { Id = 3, CanteenId = canteens.ElementAt(1).Id },
                new CanteenEmployee("e3456789", "Lennart de Groot") { Id = 4, CanteenId = canteens.ElementAt(3).Id }
            };

            IEnumerable<Student> students = new List<Student>
            {
                new Student("adminmail@avans.nl", "a0000000", new DateTime(1998, 11, 11), "Admin", "Breda") { Id = 1 },
                new Student("je.boellaard@student.avans.nl", "s2182556", new DateTime(1998, 11, 11), "Joy Boellaard", "Breda"){ Id = 2, PhoneNr = "0612345678" },
                new Student("em.degroot@student.avans.nl", "s2192233", new DateTime(2006, 1, 31), "Emma de Groot", "Breda"){ Id = 3, PhoneNr = "0623456789", NumberOfTimesNotCollected = 2 },
                new Student("b.dejong@student.avans.nl", "s2192344", new DateTime(2001, 3, 7), "Ben de Jong", "Tilburg"){ Id = 4 },
                new Student("d.li@student.avans.nl", "s2184399", new DateTime(2005, 4, 12), "Diana Li", "Breda"){ Id = 5, PhoneNr = "0645678901" }
            };

            IEnumerable<ProductImage> productImages = new List<ProductImage>
            {
                new ProductImage(File.ReadAllBytes("./Infrastructure.AG-EF/productImages/vodka.webp")) { Id = 1 },
                new ProductImage(File.ReadAllBytes("./Infrastructure.AG-EF/productImages/panini.jpg")) { Id = 2 },
                new ProductImage(File.ReadAllBytes("./Infrastructure.AG-EF/productImages/sandwich.jpg")) { Id = 3 },
                new ProductImage(File.ReadAllBytes("./Infrastructure.AG-EF/productImages/apple.jpg")) { Id = 4 },
                new ProductImage(File.ReadAllBytes("./Infrastructure.AG-EF/productImages/soup.jpg")) { Id = 5 },
                new ProductImage(File.ReadAllBytes("./Infrastructure.AG-EF/productImages/baguette.webp")) { Id = 6 },
                new ProductImage(File.ReadAllBytes("./Infrastructure.AG-EF/productImages/bacardi.webp")) { Id = 7 },
                new ProductImage(File.ReadAllBytes("./Infrastructure.AG-EF/productImages/beer.jpg")) { Id = 8 },
                new ProductImage(File.ReadAllBytes("./Infrastructure.AG-EF/productImages/banana.webp")) { Id = 9 },
                new ProductImage(File.ReadAllBytes("./Infrastructure.AG-EF/productImages/tangerine.jpg")) { Id = 10 },
                new ProductImage(File.ReadAllBytes("./Infrastructure.AG-EF/productImages/croquette.webp")) { Id = 11 },
                new ProductImage(File.ReadAllBytes("./Infrastructure.AG-EF/productImages/cheesesouffle.jpg")) { Id = 12 }
            };

            IEnumerable<Product> products = new List<Product>
            {
                new Product("Vodka", true){ Id = 1, ProductImageId = 1 },
                new Product("Panini", false) { Id = 2, ProductImageId = 2 },
                new Product("Sandwich", false) { Id = 3, ProductImageId = 3 },
                new Product("Apple", false) { Id = 4, ProductImageId = 4 },
                new Product("Soup", false) { Id = 5, ProductImageId = 5 },
                new Product("Baguette", false) { Id = 6, ProductImageId = 6 },
                new Product("Bacardi", true) { Id = 7, ProductImageId = 7 },
                new Product("Beer", true) { Id = 8, ProductImageId = 8 },
                new Product("Banana", false) { Id = 9, ProductImageId = 9 },
                new Product("Tangerine", false) { Id = 10, ProductImageId = 10 },
                new Product("Croquette", false) { Id = 11, ProductImageId = 11 },
                new Product("Cheese Souffle", false) { Id = 12, ProductImageId = 12 }
            };

            IEnumerable<Packet> packets = new List<Packet>
            {
                new Packet("Alcoholic beverage and snack", DateTime.Now.Date.AddHours(17), DateTime.Now.Date.AddHours(20), true, 5.0m, MealTypeId.Drink, canteens.ElementAt(0).Id) { Id = 1 },
                new Packet("Lunch with two sandwiches", DateTime.Now.Date.AddDays(1).AddHours(13), DateTime.Now.Date.AddDays(1).AddHours(17), false, 5.5m, MealTypeId.Bread, canteens.ElementAt(2).Id) { Id = 2 },
                new Packet("Soup and a baguette", DateTime.Now.Date.AddDays(2).AddHours(17), DateTime.Now.Date.AddDays(2).AddHours(19), false, 4.0m, MealTypeId.WarmMeal, canteens.ElementAt(2).Id) { Id = 3 },
                new Packet("Fried snacks", DateTime.Now.Date.AddDays(1).AddHours(11), DateTime.Now.Date.AddDays(1).AddHours(15), false, 6.5m, MealTypeId.Snack, canteens.ElementAt(3).Id) { Id = 4 },
                new Packet("Left over drinks", DateTime.Now.Date.AddDays(1).AddHours(8), DateTime.Now.Date.AddDays(1).AddHours(12), true, 7m, MealTypeId.Drink, canteens.ElementAt(3).Id) { Id = 5 },
                new Packet("Fruit assortment", DateTime.Now.Date.AddDays(1).AddHours(10), DateTime.Now.Date.AddDays(1).AddHours(14), false, 6m, MealTypeId.Drink, canteens.ElementAt(2).Id) { Id = 6 }
            };

            IEnumerable<PacketProduct> productsInPacket = new List<PacketProduct>
            {
                new PacketProduct(packets.ElementAt(0).Id, products.ElementAt(0).Id){ Id = 1 },
                new PacketProduct(packets.ElementAt(0).Id, products.ElementAt(3).Id){ Id = 2 },
                new PacketProduct(packets.ElementAt(1).Id, products.ElementAt(2).Id){ Id = 3 },
                new PacketProduct(packets.ElementAt(2).Id, products.ElementAt(5).Id){ Id = 4 },
                new PacketProduct(packets.ElementAt(2).Id, products.ElementAt(6).Id){ Id = 5 },
                new PacketProduct(packets.ElementAt(3).Id, products.ElementAt(10).Id){ Id = 6 },
                new PacketProduct(packets.ElementAt(3).Id, products.ElementAt(11).Id){ Id = 7 },
                new PacketProduct(packets.ElementAt(4).Id, products.ElementAt(6).Id){ Id = 8 },
                new PacketProduct(packets.ElementAt(4).Id, products.ElementAt(7).Id){ Id = 9 },
                new PacketProduct(packets.ElementAt(5).Id, products.ElementAt(3).Id){ Id = 10 },
                new PacketProduct(packets.ElementAt(5).Id, products.ElementAt(8).Id){ Id = 11 },
                new PacketProduct(packets.ElementAt(5).Id, products.ElementAt(9).Id){ Id = 12 }
            };

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Canteen>().HasData(canteens);
            modelBuilder.Entity<CanteenEmployee>().HasData(canteenEmployees);
            modelBuilder.Entity<Student>().HasData(students);

            modelBuilder.Entity<ProductImage>().HasData(productImages);
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
                entity.HasIndex(e => e.EmployeeNr).IsUnique();
            });

            modelBuilder.Entity<Packet>().Property(p => p.Price).HasPrecision(6, 2);

            modelBuilder.Entity<MealType>().HasData(
                Enum.GetValues(typeof(MealTypeId))
                    .Cast<MealTypeId>()
                    .Select(e => new MealType()
                    {
                        MealTypeId = e,
                        Name = e.ToString()
                    })
                );

        }
    }
}
