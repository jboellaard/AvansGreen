namespace Core.Domain
{
    public class Student
    {
        public int Id { get; set; }
        public string EmailAddress { get; set; }
        public string StudentNr { get; set; }
        private DateTime _dateOfBirth;
        public DateTime DateOfBirth
        {
            get { return _dateOfBirth; }
            set
            {
                var studentBirthdayPlus16 = _dateOfBirth.AddYears(16);
                if (studentBirthdayPlus16.Date <= DateTime.Now.Date) _dateOfBirth = value;
                else if (_dateOfBirth.Date >= DateTime.Now.Date)
                    throw new InvalidOperationException("Birthday cannot be in the future.");
                else
                    throw new InvalidOperationException("Student must be at least 16 years old.");
            }
        }
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
