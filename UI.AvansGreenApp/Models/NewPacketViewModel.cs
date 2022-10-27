using Core.Domain;
using System.ComponentModel.DataAnnotations;

namespace UI.AvansGreenApp.Models
{
    public class NewPacketViewModel
    {
        [Required]
        public string? PacketName { get; set; }
        [Required]
        public string? PickUpDaysFromNow { get; set; }
        [Required]
        public DateTime PickUpTimeStart { get; set; }
        public DateTime PickUpTimeEnd { get; set; }
        [Required]
        public bool IsAlcoholic { get; set; }
        [Required]
        public decimal Price { get; set; } = 5.0m;
        public MealTypeId TypeOfMeal { get; set; }
        //public int CanteenId { get; set; }

        public ICollection<int> ProductIdList { get; set; } = new List<int>();
        public ICollection<Product> Products { get; set; } = new List<Product>();

        public ICollection<Product> AllProducts { get; set; } = new List<Product>();



    }
}
