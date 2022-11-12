using Core.Domain;
using System.Runtime.Serialization;

namespace UI.AG_StudentReservationsAPI.Models
{
    [DataContract]
    public class ProductDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsAlcoholic { get; set; }
        public int ProductImageId { get; set; }
        public byte[] ImageData { get; set; }
        public string ImageFormat { get; set; } = "png";

        public ProductDTO(Product product)
        {
            Id = product.Id;
            Name = product.Name;
            IsAlcoholic = product.IsAlcoholic;
            ProductImageId = product.ProductImageId;
            ImageData = product.ProductImage.ImageData;
            ImageFormat = product.ProductImage.ImageFormat;
        }

        public ProductDTO() { }
    }
}
