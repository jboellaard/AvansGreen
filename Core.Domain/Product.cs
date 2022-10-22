namespace Core.Domain
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsAlcoholic { get; set; } = false;
        public byte[]? ImageData { get; set; }

        public Product(string name)
        {
            Name = name;
        }
    }
}
