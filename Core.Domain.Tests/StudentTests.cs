using System.ComponentModel.DataAnnotations;

namespace Core.Domain.Tests
{
    public class StudentTests
    {

        [Fact]
        public void StudentYoungerThan16ShouldGiveValidationError()
        {
            //Arrange
            var student = new Student("email@address.com", "s0000000", DateTime.Now.AddDays((int)(-365.25 * 16) + 1), "Name", "Breda");

            //Act
            var results = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(
            student,
            new ValidationContext(student, null, null), results);

            //Assert
            Assert.False(isValid);
            //Trace.WriteLine(results.Count);
            //foreach (ValidationResult result in results) Trace.WriteLine(result.ErrorMessage);
            Assert.Single(results);
            Assert.Equal("Student must be 16 years or older.", results.ElementAt(0).ErrorMessage);
        }

        [Fact]
        public void StudentDateOfBirthInTheFutureShouldGiveValidationError()
        {
            //Arrange
            var student = new Student("email@address.com", "s0000000", DateTime.Now.AddDays(1), "Name", "Breda");

            //Act
            var results = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(
            student,
            new ValidationContext(student, null, null), results);

            Assert.False(isValid);
            Assert.Contains(results, item => item.ErrorMessage == "Date of birth cannot be in the future.");
        }

        [Fact]
        public void no()
        {
            //Arrange
            var student = new Student("email@address.com", "s0000000", DateTime.Now.AddDays(1), "Name", "Breda");

            //Act
            var results = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(
            student,
            new ValidationContext(student, null, null), results);

            Assert.False(isValid);
            Assert.Contains(results, item => item.ErrorMessage == "Date of birth cannot be in the future.");
        }

        [Fact]
        public void StudentOlderOrEqualTo16MustBeValid()
        {
            //Arrange
            var student = new Student("email@address.com", "s0000000", DateTime.Now.AddDays((int)(-365.25 * 16)), "Name", "Breda");

            //Act
            var results = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(
            student,
            new ValidationContext(student, null, null), results);

            //Assert
            Assert.True(isValid);
            Assert.Empty(results);
        }
    }
}