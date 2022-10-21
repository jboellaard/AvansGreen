using Core.Domain;
using Core.DomainServices.IRepos;

namespace Infrastructure.AG_EF
{
    public class StudentEFRepository : IStudentRepository
    {

        private readonly AvansGreenDbContext _context;

        public StudentEFRepository(AvansGreenDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Student> GetStudents()
        {
            return _context.Students;
        }

        public Student? GetById(int id)
        {
            return _context.Students.SingleOrDefault(student => student.Id == id);
        }

        public Student? GetByEmail(string email)
        {
            return _context.Students.SingleOrDefault(student => student.EmailAddress == email);
        }

    }
}
