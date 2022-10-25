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

        public DbSet<MealType> MealTypes { get; set; }



        public AvansGreenDbContext(DbContextOptions<AvansGreenDbContext> contextOptions) : base(contextOptions)
        {
            Canteens = Set<Canteen>();
            CanteenEmployees = Set<CanteenEmployee>();
            Packets = Set<Packet>();
            Products = Set<Product>();
            Students = Set<Student>();
            ProductsInPacket = Set<PacketProduct>();
            MealTypes = Set<MealType>();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //IEnumerable<Canteen> canteens = Canteen.GetAll<Canteen>();

            IEnumerable<BredaCanteen> bredaCanteens = new List<BredaCanteen>
            {
                BredaCanteen.LA5,
                BredaCanteen.LD1,
                BredaCanteen.HA1
            };

            IEnumerable<TilburgCanteen> tilburgCanteens = new List<TilburgCanteen>
            {
                TilburgCanteen.TH1,
                TilburgCanteen.TH5
            };

            IEnumerable<DenBoschCanteen> denBoschCanteens = new List<DenBoschCanteen>
            {
                DenBoschCanteen.DH1,
                DenBoschCanteen.DH5
            };

            IEnumerable<CanteenEmployee> canteenEmployees = new List<CanteenEmployee>
            {
                new CanteenEmployee("a0000000", "Admin") { Id = 1, CanteenId = bredaCanteens.ElementAt(2).Id },
                new CanteenEmployee("e1234567", "Naomi de Vries") { Id = 2, CanteenId = bredaCanteens.ElementAt(0).Id },
                new CanteenEmployee("e2345678", "Peter Smit") { Id = 3, CanteenId = bredaCanteens.ElementAt(1).Id },
                new CanteenEmployee("e3456789", "Lennart de Groot") { Id = 4, CanteenId = tilburgCanteens.ElementAt(0).Id }
            };

            IEnumerable<Student> students = new List<Student>
            {
                new Student("adminmail@avans.nl", "a0000000", new DateTime(1998, 11, 11), "Admin", "Breda") { Id = 1 },
                new Student("je.boellaard@student.avans.nl", "s2182556", new DateTime(1998, 11, 11), "Joy Boellaard", "Breda"){ Id = 2, PhoneNr = "0612345678" },
                new Student("em.degroot@student.avans.nl", "s2192233", new DateTime(2000, 1, 31), "Emma de Groot", "Breda"){ Id = 3, PhoneNr = "0623456789" },
                new Student("b.dejong@student.avans.nl", "s2192344", new DateTime(2001, 3, 7), "Ben de Jong", "Tilburg"){ Id = 4 },
                new Student("d.li@student.avans.nl", "s2184399", new DateTime(1999, 4, 12), "Diana Li", "Breda"){ Id = 5, PhoneNr = "0645678901" }
            };

            IEnumerable<Product> products = new List<Product>
            {
                new Product("Bottle of vodka", true){ Id = 1 },
                new Product("Panini", false) { Id = 2 },
                new Product("Sandwich", false) { Id = 3 },
                new Product("Apple", false) { Id = 4 },
                new Product("Soup", false) { Id = 5 },
                new Product("Baguette", false) { Id = 6 }
            };

            IEnumerable<Packet> packets = new List<Packet>
            {
                new Packet("Alcoholic beverage and snack", new DateTime(2022, 10, 20, 17, 0, 0), new DateTime(2022, 10, 20, 20, 0, 0), true, 5.0m, MealTypeId.Drink, bredaCanteens.ElementAt(0).Id) { Id = 1 },
                new Packet("Lunch with two sandwiches", new DateTime(2022, 10, 21, 13, 0, 0), new DateTime(2022, 10, 21, 17, 0, 0), false, 5.5m, MealTypeId.Bread, bredaCanteens.ElementAt(1).Id) { Id = 2 }
            };

            IEnumerable<PacketProduct> productsInPacket = new List<PacketProduct>
            {
                new PacketProduct(packets.ElementAt(0).Id, products.ElementAt(0).Id){ Id = 1 },
                new PacketProduct(packets.ElementAt(0).Id, products.ElementAt(3).Id){ Id = 2 },
                new PacketProduct(packets.ElementAt(1).Id, products.ElementAt(2).Id){ Id = 3 },
            };

            base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<Canteen>().HasData(canteens);
            modelBuilder.Entity<BredaCanteen>().HasData(bredaCanteens);
            modelBuilder.Entity<TilburgCanteen>().HasData(tilburgCanteens);
            modelBuilder.Entity<DenBoschCanteen>().HasData(denBoschCanteens);

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
