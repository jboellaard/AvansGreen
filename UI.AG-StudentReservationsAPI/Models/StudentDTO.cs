using Core.Domain;
using System.Runtime.Serialization;

namespace UI.AG_StudentReservationsAPI.Models
{
    [DataContract]
    public class StudentDTO
    {
        public int Id { get; set; }
        public string EmailAddress { get; set; }
        public string StudentNr { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Name { get; set; }
        public string CityOfSchool { get; set; }
        public string PhoneNr { get; set; }
        public int NumberOfTimesNotCollected { get; set; }

        public StudentDTO(Student student)
        {
            Id = student.Id;
            EmailAddress = student.EmailAddress;
            StudentNr = student.StudentNr;
            DateOfBirth = student.DateOfBirth;
            Name = student.Name;
            CityOfSchool = student.CityOfSchool;
            PhoneNr = student.PhoneNr;
        }

        public StudentDTO() { }
    }
}
