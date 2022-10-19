namespace Core.Domain
{
    class Student
    {
        public string EmailAddress { get; set; }
        public string StudentNr { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string FullName { get; set; }
        public City CityOfSchool { get; set; }
        public string PhoneNr { get; set; }
    }
}
