using Core.Domain;
namespace UI.AvansGreenPortal.Models
{
    public class PacketDetailViewModel
    {
        public Packet Packet { get; set; }

        public bool CanEdit { get; set; } = false;

        public CurrentUserViewModel CurrentUser { get; set; } = new CurrentUserViewModel();

    }
}
