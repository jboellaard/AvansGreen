using Core.Domain;

namespace UI.AvansGreenApp.Models
{
    public class FilterPacketsViewModel
    {
        public List<Packet> Packets { get; set; } = new List<Packet>();
        public ICollection<int> CanteenIdList { get; set; } = new List<int>();

        public MealTypeId TypeOfMeal { get; set; }
    }
}
