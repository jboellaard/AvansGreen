using System.Runtime.Serialization;

namespace Core.Domain
{
    [DataContract]
    public class Packet
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public DateTime PickUpTimeStart { get; set; }
        [DataMember]
        public DateTime PickUpTimeEnd { get; set; }
        [IgnoreDataMember]
        public DateTime? TimeOfPickUpByStudent { get; set; }
        [DataMember]
        public bool IsAlcoholic { get; set; }
        [DataMember]
        public decimal Price { get; set; }
        [DataMember]
        public int CanteenId { get; set; }
        [IgnoreDataMember]
        public Canteen Canteen { get; set; } = null!;
        [DataMember]
        public MealTypeId MealTypeId { get; set; }
        [IgnoreDataMember]
        public MealType? MealType { get; set; }
        [DataMember]
        public int? StudentId { get; set; }
        [IgnoreDataMember]
        public Student? Student { get; set; }
        [IgnoreDataMember]
        public ICollection<PacketProduct> Products { get; set; } = new List<PacketProduct>();

        public Packet(string name, DateTime pickUpTimeStart, DateTime pickUpTimeEnd, bool isAlcoholic, decimal price, MealTypeId mealTypeId, int canteenId)
        {
            Name = name;
            PickUpTimeStart = pickUpTimeStart;
            PickUpTimeEnd = pickUpTimeEnd;
            IsAlcoholic = isAlcoholic;
            Price = price;
            MealTypeId = mealTypeId;
            CanteenId = canteenId;
        }

        public Packet(string name, DateTime pickUpTimeStart, DateTime pickUpTimeEnd, bool isAlcoholic, decimal price, MealTypeId mealTypeId, Canteen canteen)
            : this(name, pickUpTimeStart, pickUpTimeEnd, isAlcoholic, price, mealTypeId, canteen.Id)
        {
            Canteen = canteen;
        }

        public Packet() { }
    }
}