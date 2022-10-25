namespace Core.Domain
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsAlcoholic { get; set; }
        public byte[]? ImageData { get; set; }

        public Product(string name, bool isAlcoholic)
        {
            Name = name;
            IsAlcoholic = isAlcoholic;
        }
    }
}
