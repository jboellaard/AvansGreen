namespace Core.Domain
{
    public class Packet
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime PickUpTimeStart { get; set; }
        public DateTime PickUpTimeEnd { get; set; }
        public DateTime? TimeOfPickUpByStudent { get; set; }
        public bool IsAlcoholic { get; set; }
        public decimal Price { get; set; }
        public int CanteenId { get; set; }
        public Canteen Canteen { get; set; } = null!;
        public MealTypeId MealTypeId { get; set; }
        public MealType? MealType { get; set; }
        public int? StudentId { get; set; }
        public Student? Student { get; set; }
        public ICollection<PacketProduct> Products { get; set; } = new List<PacketProduct>();

        public Packet(string name, DateTime pickUpTimeStart, DateTime pickUpTimeEnd, bool isAlcoholic, decimal price, MealTypeId mealTypeId, int canteenId)
        {
            Name = name;
            PickUpTimeStart = pickUpTimeStart;
            PickUpTimeEnd = pickUpTimeEnd;
            IsAlcoholic = isAlcoholic;
            Price = price;
            MealTypeId = mealTypeId;
            //MealType = new MealType() { MealTypeId = MealTypeId, Name = MealTypeId.ToString() };
            CanteenId = canteenId;
        }

        public Packet(string name, DateTime pickUpTimeStart, DateTime pickUpTimeEnd, bool isAlcoholic, decimal price, MealTypeId mealTypeId, Canteen canteen)
            : this(name, pickUpTimeStart, pickUpTimeEnd, isAlcoholic, price, mealTypeId, canteen.Id)
        {
            Canteen = canteen;
        }

    }
}