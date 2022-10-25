using System.ComponentModel.DataAnnotations;

namespace Core.Domain
{
    public class Student : IValidatableObject
    {
        public int Id { get; set; }
        [EmailAddress]
        public string EmailAddress { get; set; }
        public string StudentNr { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Name { get; set; }
        public string CityOfSchool { get; set; }
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

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (DateOfBirth.Date > DateTime.Now.Date)
            {
                yield return new ValidationResult("Date of birth cannot be in the future.", new[] { nameof(DateOfBirth) });
            }

            if (DateOfBirth.AddYears(16).Date > DateTime.Now.Date)
            {
                yield return new ValidationResult("Student must be 16 years or older.", new[] { nameof(DateOfBirth) });
            }

            if (StudentNr.Length > 7)
            {
                yield return new ValidationResult("Student nr must be 7 digits.", new[] { nameof(StudentNr) });
            }

        }
    }
}
