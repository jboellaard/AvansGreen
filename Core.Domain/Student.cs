namespace Core.Domain
{
    public class Student
    {
        public int Id { get; set; }
        public string EmailAddress { get; set; }
        public string StudentNr { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string FullName { get; set; }
        public City CityOfSchool { get; set; }
        public string? PhoneNr { get; set; }
        public ICollection<Packet> ReservedPackets { get; set; } = new List<Packet>();

        public Student(string emailAddress, string studentNr, DateTime dateOfBirth, string fullName, City cityOfSchool)
        {
            EmailAddress = emailAddress;
            StudentNr = studentNr;
            DateOfBirth = dateOfBirth;
            FullName = fullName;
            CityOfSchool = cityOfSchool;
        }


    }
}
