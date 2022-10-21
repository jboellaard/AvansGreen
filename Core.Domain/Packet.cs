namespace Core.Domain
{
    public class Packet
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CanteenId { get; set; }
        public Canteen Canteen { get; set; }
        public DateTime PickUpTimeStart { get; set; }
        public DateTime PickUpTimeEnd { get; set; }
        public DateTime TimeOfPickUpByStudent { get; set; }
        public bool IsAlcoholic { get; set; }
        public double Price { get; set; }
        public MealType TypeOfMeal { get; set; }
        public int? StudentId { get; set; }
        public Student? ReservedBy { get; set; } = null;
        public ICollection<PacketProduct> Products { get; set; }
    }
}