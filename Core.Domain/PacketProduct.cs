namespace Core.Domain
{
    public class PacketProduct
    {
        public int Id { get; set; }
        public int PacketId { get; set; }
        public Packet Packet { get; set; } = null!;
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;


        public PacketProduct(Packet packet, Product product)
        {
            Packet = packet;
            PacketId = packet.Id;
            Product = product;
            ProductId = product.Id;
        }

        public PacketProduct(int packetId, int productId)
        {
            PacketId = packetId;
            ProductId = productId;
        }
    }
}
