using Core.Domain;
using System.Runtime.Serialization;

namespace UI.AG_StudentReservationsAPI.Models
{
    [DataContract]
    public class CanteenDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string City { get; set; }

        public bool HasWarmMeals { get; set; }
        public CanteenDTO(Canteen canteen)
        {
            Id = canteen.Id;
            Name = canteen.Name;
            City = canteen.City;
            HasWarmMeals = canteen.HasWarmMeals;
        }

        public CanteenDTO() { }
    }
}
