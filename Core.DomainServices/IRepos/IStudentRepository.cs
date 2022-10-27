using Core.Domain;

namespace Core.DomainServices.IRepos
{
    public interface IStudentRepository
    {
        IEnumerable<Student> GetStudents();
        Student? GetById(int id);
        Student? GetByStudentNr(string studentNr);
    }
}
