using Core.Domain;
namespace UI.AvansGreenApp.Models
{
    public class PacketDetailViewModel
    {
        public Packet Packet { get; set; }

        public bool CanEdit { get; set; } = false;

    }
}
