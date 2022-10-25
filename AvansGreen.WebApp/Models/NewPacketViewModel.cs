using Core.Domain;
using System.ComponentModel.DataAnnotations;

namespace AvansGreen.WebApp.Models
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
        public CanteenEmployee? CanteenEmployee { get; set; }
        public ICollection<Product> Products { get; set; } = new List<Product>();

        public ICollection<Product> AllProducts { get; set; } = new List<Product>();



    }
}
