using Core.Domain;

namespace UI.AvansGreenPortal.Models
{
    public class FilterPacketsViewModel
    {
        public List<Packet> Packets { get; set; } = new List<Packet>();
        public ICollection<string> CityList { get; set; } = new List<string>();

        public List<string> CityOptions { get; set; } = new List<string>() { "Breda", "Tilburg", "Den Bosch" };

        public MealTypeId TypeOfMeal { get; set; }

        public CurrentUserViewModel CurrentUser { get; set; } = new CurrentUserViewModel();
    }
}
