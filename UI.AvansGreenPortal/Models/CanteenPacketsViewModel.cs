using Core.Domain;

namespace UI.AvansGreenPortal.Models
{
    public class CanteenPacketsViewModel
    {
        public Canteen Canteen { get; set; }
        public List<Packet> Packets { get; set; } = new List<Packet>();
    }
}
