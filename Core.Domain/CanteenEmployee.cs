namespace Core.Domain
{
    public class CanteenEmployee : AGUser
    {
        public int Id { get; set; }
        //public string EmailAddress { get; set; }
        public string EmployeeNr { get; set; }
        public int CanteenId { get; set; }
        public Canteen Canteen { get; set; }

        public CanteenEmployee(string emailAddress) : base(emailAddress) { }
    }
}
