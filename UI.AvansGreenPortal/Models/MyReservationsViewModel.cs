using Core.Domain;
namespace UI.AvansGreenPortal.Models
{
    public class MyReservationsViewModel
    {
        public List<Packet> Reservations { get; set; } = new List<Packet>();

        public CurrentUserViewModel CurrentUser { get; set; } = new CurrentUserViewModel();
    }
}
