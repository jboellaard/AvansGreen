using Core.Domain;

namespace AvansGreen.WebApp.Models
{
    public class CurrentUserViewModel
    {
        public string Email { get; set; } = "Anonymous";
        public TypeOfUser TypeOfUser { get; set; } = TypeOfUser.Anonymous;
        public CanteenEmployee? CanteenEmployee { get; set; }
        public Student? Student { get; set; }
    }
}
