namespace AvansGreen.WebApp.Models
{
    public class CurrentUserViewModel
    {
        public string Name { get; set; } = "Anonymous";
        public string Nr { get; set; } = "";
        public TypeOfUser TypeOfUser { get; set; } = TypeOfUser.Anonymous;
    }
}
