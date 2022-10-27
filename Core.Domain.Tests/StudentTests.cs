using System.Diagnostics;

namespace Core.Domain.Tests
{
    public class StudentTests
    {

        public static readonly object[][] CorrectDateData =
            {
                new object[] {"email@address.com", "s0000000", DateTime.Now.AddDays((int)(-365.25 * 16) - 1), "Name", "Breda" },
                new object[] {"email@address.com", "s0000000", DateTime.Now.AddDays((int)(-365.25 * 16)), "Name", "Breda"}
            };

        public static readonly object[][] CorrectCityData =
            {
                new object[] {"email@address.com", "s0000000", DateTime.Now.AddDays((int)(-365.25 * 16)), "Name", "Breda" },
                new object[] {"email@address.com", "s0000000", DateTime.Now.AddDays((int)(-365.25 * 16)), "Name", "breda" },
                new object[] {"email@address.com", "s0000000", DateTime.Now.AddDays((int)(-365.25 * 16)), "Name", "Tilburg" },
                new object[] {"email@address.com", "s0000000", DateTime.Now.AddDays((int)(-365.25 * 16)), "Name", "tilburg" },
                new object[] {"email@address.com", "s0000000", DateTime.Now.AddDays((int)(-365.25 * 16)), "Name", "Den Bosch" },
                new object[] {"email@address.com", "s0000000", DateTime.Now.AddDays((int)(-365.25 * 16)), "Name", "den bosch" }
            };

        [Fact]
        public void Student_Younger_Than_16_Should_Throw_Exception()
        {
            //Arrange
            string email = "email@address.com";
            string studentNr = "s0000000";
            DateTime dateOfBirth = DateTime.Now.AddDays((int)(-365.25 * 16) + 1);
            string name = "Name";
            string cityOfSchool = "Breda";

            bool exceptionThrown = false;
            string message = "";

            //Act
            try
            {
                Student student = new Student(email, studentNr, dateOfBirth, name, cityOfSchool);
            }
            catch (InvalidOperationException e)
            {
                exceptionThrown = true;
                message = e.Message;
            }

            //Assert
            Assert.True(exceptionThrown);
            Assert.Equal("Student must be at least 16 years old", message);
        }

        [Fact]
        public void Student_Date_Of_Birth_In_The_Future_Should_Throw_Exception()
        {
            //Arrange
            string email = "email@address.com";
            string studentNr = "s0000000";
            DateTime dateOfBirth = DateTime.Now.AddDays(1);
            string name = "Name";
            string cityOfSchool = "Breda";

            bool exceptionThrown = false;
            string message = "";

            //Act
            try
            {
                Student student = new Student(email, studentNr, dateOfBirth, name, cityOfSchool);
            }
            catch (InvalidOperationException e)
            {
                exceptionThrown = true;
                message = e.Message;
            }

            //Assert
            Assert.True(exceptionThrown);
            Assert.Equal("Date of birth cannot be in the future", message);
        }

        [Theory, MemberData(nameof(CorrectDateData))]
        public void Correct_Date_Of_Birth_Should_Not_Throw_Exception(string email, string studentNr, DateTime dateOfBirth, string name, string cityOfSchool)
        {
            //Arrange
            bool exceptionThrown = false;
            Student? student = null;

            //Act
            try
            {
                student = new Student(email, studentNr, dateOfBirth, name, cityOfSchool);
            }
            catch (InvalidOperationException e)
            {
                exceptionThrown = true;
                Trace.Write(e.Message);
            }

            //Assert
            Assert.False(exceptionThrown);
            Assert.True(student != null);
        }

        [Theory, MemberData(nameof(CorrectDateData))]
        public void Valid_Cities_Should_Not_Throw_Exception_Regardless_Capitalization(string email, string studentNr, DateTime dateOfBirth, string name, string cityOfSchool)
        {
            //Arrange
            bool exceptionThrown = false;
            Student? student = null;

            //Act
            try
            {
                student = new Student(email, studentNr, dateOfBirth, name, cityOfSchool);
            }
            catch (InvalidOperationException e)
            {
                exceptionThrown = true;
                Trace.Write(e.Message);
            }

            //Assert
            Assert.False(exceptionThrown);
            Assert.True(student != null);
        }
    }
}