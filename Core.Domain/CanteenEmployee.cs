namespace Core.Domain
{
    public class CanteenEmployee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string EmployeeNr { get; set; }
        public int CanteenId { get; set; }
        public Canteen? Canteen { get; set; }
        public CanteenEmployee(string employeeNr, string name)
        {
            EmployeeNr = employeeNr;
            Name = name;
        }
    }
}
