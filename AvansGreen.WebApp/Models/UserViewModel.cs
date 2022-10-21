using Core.Domain;

namespace AvansGreen.WebApp.Models
{
    public class UserViewModel
    {
        public TypeOfUser TypeOfUser { get; set; } = TypeOfUser.Anonymous;
        public string DisplayName { get; set; } = "Anonymous";

        public List<Packet> Packets { get; set; } = new List<Packet>();

    }
}
