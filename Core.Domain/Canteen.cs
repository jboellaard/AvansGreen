namespace Core.Domain
{
    public class Canteen
    {
        public int Id { get; set; }
        public string Location { get; set; }
        public City City { get; set; }

        public Canteen(string location, City city)
        {
            Location = location;
            City = city;
        }
    }
}
