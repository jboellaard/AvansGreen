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

            // // Only works locally because of the paths, but images are added in a migration
            //IEnumerable<ProductImage> productImages = new List<ProductImage>
            //{
            //    new ProductImage(File.ReadAllBytes("../Infrastructure.AG-EF/productImages/vodka.webp")) { Id = 1 },
            //    new ProductImage(File.ReadAllBytes("../Infrastructure.AG-EF/productImages/panini.jpg")) { Id = 2 },
            //    new ProductImage(File.ReadAllBytes("../Infrastructure.AG-EF/productImages/sandwich.jpg")) { Id = 3 },
            //    new ProductImage(File.ReadAllBytes("../Infrastructure.AG-EF/productImages/apple.jpg")) { Id = 4 },
            //    new ProductImage(File.ReadAllBytes("../Infrastructure.AG-EF/productImages/soup.jpg")) { Id = 5 },
            //    new ProductImage(File.ReadAllBytes("../Infrastructure.AG-EF/productImages/baguette.webp")) { Id = 6 },
            //    new ProductImage(File.ReadAllBytes("../Infrastructure.AG-EF/productImages/bacardi.webp")) { Id = 7 },
            //    new ProductImage(File.ReadAllBytes("../Infrastructure.AG-EF/productImages/beer.jpg")) { Id = 8 },
            //    new ProductImage(File.ReadAllBytes("../Infrastructure.AG-EF/productImages/banana.webp")) { Id = 9 },
            //    new ProductImage(File.ReadAllBytes("../Infrastructure.AG-EF/productImages/tangerine.jpg")) { Id = 10 },
            //    new ProductImage(File.ReadAllBytes("../Infrastructure.AG-EF/productImages/croquette.webp")) { Id = 11 },
            //    new ProductImage(File.ReadAllBytes("../Infrastructure.AG-EF/productImages/cheesesouffle.jpg")) { Id = 12 }
            //};

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Canteen>().HasData(canteens);

            //modelBuilder.Entity<ProductImage>().HasData(productImages);

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
