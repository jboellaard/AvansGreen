using Core.Domain;
using System.Runtime.Serialization;

namespace UI.AG_StudentReservationsAPI.Models
{
    [DataContract]
    public class PacketDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime PickUpTimeStart { get; set; }
        public DateTime PickUpTimeEnd { get; set; }
        public DateTime TimeOfPickUpByStudent { get; set; }
        public bool IsAlcoholic { get; set; }
        public decimal Price { get; set; }
        public int CanteenId { get; set; }
        public CanteenDTO Canteen { get; set; }
        public MealTypeId MealTypeId { get; set; }
        public MealType MealType { get; set; }
        public int StudentId { get; set; }
        public StudentDTO Student { get; set; }
        public List<ProductDTO> Products { get; set; } = new List<ProductDTO>();

        public PacketDTO() { }

        public PacketDTO(Packet packet)
        {
            Id = packet.Id;
            Name = packet.Name;
            PickUpTimeStart = packet.PickUpTimeStart;
            PickUpTimeEnd = packet.PickUpTimeEnd;
            if (packet.TimeOfPickUpByStudent != null)
                TimeOfPickUpByStudent = (DateTime)packet.TimeOfPickUpByStudent;
            IsAlcoholic = packet.IsAlcoholic;
            Price = packet.Price;
            CanteenId = packet.CanteenId;
            if (packet.Canteen != null) Canteen = new CanteenDTO(packet.Canteen);
            MealTypeId = packet.MealTypeId;
            if (packet.MealType != null) MealType = (MealType)packet.MealType;
            if (packet.StudentId != null) StudentId = (int)packet.StudentId;
            if (packet.Student != null) Student = new StudentDTO(packet.Student);
            foreach (PacketProduct packetProduct in packet.Products)
            {
                Products.Add(new ProductDTO(packetProduct.Product));
            }

        }

    }
}
