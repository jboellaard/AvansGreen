using Core.Domain;
namespace AvansGreen.WebApp.Models
{
    public class ProductCheckListViewModel
    {
        public Product Product { get; set; }

        public bool Selected { get; set; } = false;

        public ProductCheckListViewModel(Product product)
        {
            Product = product;
        }


    }
}
