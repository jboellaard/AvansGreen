namespace Core.Domain
{
    public class ProductImage
    {
        public int Id { get; set; }

        public byte[] ImageData { get; set; }

        public string? ImageFormat { get; set; } = "png";

        public ProductImage(byte[] imageData)
        {
            ImageData = imageData;
        }

        public string GetSrc(string? format = null)
        {
            format ??= ImageFormat;
            return $"data:{format};base64,{Convert.ToBase64String(ImageData)}";
        }
    }
}
