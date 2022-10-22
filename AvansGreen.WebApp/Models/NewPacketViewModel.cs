using Core.Domain;
using System.ComponentModel.DataAnnotations;

namespace AvansGreen.WebApp.Models
{
    public class NewPacketViewModel
    {
        [Required]
        public string? Name { get; set; }
        [Required]
        public DateOnly DateOfPickUp { get; set; }
        [Required]
        public DateTime PickUpTimeStart { get; set; }
        public DateTime PickUpTimeEnd { get; set; }
        [Required]
        public bool IsAlcoholic { get; set; }
        [Required]
        public decimal Price { get; set; }
        public MealType TypeOfMeal { get; set; }
        [Required]
        public CanteenEmployee? CanteenEmployee { get; set; }
        public ICollection<Product> Products { get; set; } = new List<Product>();

        public ICollection<ProductCheckListViewModel> AllProducts { get; set; } = new List<ProductCheckListViewModel>();



    }
}
