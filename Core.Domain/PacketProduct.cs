namespace Core.Domain
{
    public class PacketProduct
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int PacketId { get; set; }
        public Packet Packet { get; set; }
    }
}
