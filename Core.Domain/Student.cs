using System.ComponentModel.DataAnnotations;

namespace Core.Domain
{
    public class Student
    {
        public int Id { get; set; }
        [EmailAddress]
        public string EmailAddress { get; set; }
        public string StudentNr { get; set; }
        private DateTime _dateOfBirth;
        public DateTime DateOfBirth
        {
            get => _dateOfBirth;
            set
            {
                if (value.Date > DateTime.Now.Date) throw new InvalidOperationException("Date of birth cannot be in the future");
                else if (value.AddYears(16) > DateTime.Now.Date) throw new InvalidOperationException("Student must be at least 16 years old");
                else _dateOfBirth = value;
            }
        }
        public string Name { get; set; }
        private string _cityOfSchool = null!;
        public string CityOfSchool
        {
            get => _cityOfSchool;
            set
            {
                string[] cities = new string[] { "breda", "tilburg", "den bosch" };
                if (cities.Contains(value.ToLower())) _cityOfSchool = value;
                else throw new InvalidOperationException("City of school must be one of: " + string.Join(", ", cities));
            }
        }
        [Phone]
        public string? PhoneNr { get; set; }
        public ICollection<Packet> ReservedPackets { get; set; } = new List<Packet>();

        public Student(string emailAddress, string studentNr, DateTime dateOfBirth, string name, string cityOfSchool)
        {
            EmailAddress = emailAddress;
            StudentNr = studentNr;
            DateOfBirth = dateOfBirth;
            Name = name;
            CityOfSchool = cityOfSchool;
        }
    }
}
