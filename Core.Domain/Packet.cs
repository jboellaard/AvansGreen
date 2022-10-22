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
        public MealType TypeOfMeal { get; set; }
        public int CanteenEmployeeId { get; set; }
        //public CanteenEmployee CanteenEmployee { get; set; }
        private CanteenEmployee? _canteenEmployee;
        public CanteenEmployee CanteenEmployee
        {
            get { return _canteenEmployee ?? throw new InvalidOperationException("Uninitialized property: " + nameof(CanteenEmployee)); }
            set { _canteenEmployee = value; }
        }
        public int? StudentId { get; set; }
        public Student? Student { get; set; }
        public ICollection<PacketProduct> Products { get; set; } = new List<PacketProduct>();

        public Packet(string name, DateTime pickUpTimeStart, DateTime pickUpTimeEnd, bool isAlcoholic, decimal price, MealType typeOfMeal, CanteenEmployee canteenEmployee)
        {
            Name = name;
            PickUpTimeStart = pickUpTimeStart;
            PickUpTimeEnd = pickUpTimeEnd;
            IsAlcoholic = isAlcoholic;
            Price = price;
            TypeOfMeal = typeOfMeal;
            CanteenEmployee = canteenEmployee;
        }

        public Packet(string name, DateTime pickUpTimeStart, DateTime pickUpTimeEnd, bool isAlcoholic, decimal price, MealType typeOfMeal, int canteenEmployeeId)
        {
            Name = name;
            PickUpTimeStart = pickUpTimeStart;
            PickUpTimeEnd = pickUpTimeEnd;
            IsAlcoholic = isAlcoholic;
            Price = price;
            TypeOfMeal = typeOfMeal;
            CanteenEmployeeId = canteenEmployeeId;
        }

    }
}