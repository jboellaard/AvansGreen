namespace Core.Domain
{
    public class Student : AGUser
    {
        public int Id { get; set; }
        //public string EmailAddress { get; set; }
        public string StudentNr { get; set; }
        public DateTime DateOfBirth { get; set; }
        //public string FullName { get; set; }
        public City CityOfSchool { get; set; }
        public string? PhoneNr { get; set; }

        public Student(string emailAddress) : base(emailAddress) { }

        public Student(string emailAddress, string fullName) : base(emailAddress, fullName) { }

    }
}
