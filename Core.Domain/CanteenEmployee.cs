namespace Core.Domain
{
    public class CanteenEmployee
    {
        public int Id { get; set; }
        public string EmailAddress { get; set; }
        public string EmployeeNr { get; set; }
        public int CanteenId { get; set; }
        public Canteen? Canteen { get; set; }
        public ICollection<Packet> CreatedPackets { get; set; } = new List<Packet>();

        public CanteenEmployee(string employeeNr, string emailAddress)
        {
            EmployeeNr = employeeNr;
            EmailAddress = emailAddress;
        }
    }
}
