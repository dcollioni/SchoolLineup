namespace SchoolLineup.Web.Mvc.Controllers.Queries.Student
{
    using Raven.Client;
    using Raven.Client.Linq;
    using SchoolLineup.Domain.Entities;
    using SchoolLineup.Web.Mvc.Controllers.ViewModels;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;

    public class StudentListQuery : IStudentListQuery
    {
        private readonly IDocumentSession session;

        public StudentListQuery(IDocumentSession session)
        {
            this.session = session;
        }

        public Student Get(string email, string password)
        {
            return session.Query<Student>()
                          .Where(s => s.Email == email && s.Password == password)
                          .SingleOrDefault();
        }

        public Student Get(int id)
        {
            return session.Load<Student>(id);
        }

        public IEnumerable<StudentViewModel> GetAll()
        {
            return session.Query<Student>()
                          .Select(s => new StudentViewModel()
                          {
                              Email = s.Email,
                              Id = s.Id,
                              Name = s.Name,
                              Password = s.Password,
                              RegistrationCode = s.RegistrationCode
                          })
                          .OrderBy(s => s.Name)
                          .ToList<StudentViewModel>();
        }

        public IEnumerable<StudentViewModel> GetAll(string query)
        {
            return session.Query<Student>()
                          .Where(s => s.Name.StartsWith(query) || s.Email.StartsWith(query))
                          .Select(s => new StudentViewModel()
                          {
                              Email = s.Email,
                              Id = s.Id,
                              Name = s.Name,
                              Password = s.Password,
                              RegistrationCode = s.RegistrationCode
                          })
                          .OrderBy(s => s.Name)
                          .ToList<StudentViewModel>();
        }

        public IEnumerable<StudentViewModel> GetAllByCourse(int courseId)
        {
            var viewModels = new List<StudentViewModel>();

            var course = session.Query<Course>()
                                     .Where(c => c.Id == courseId)
                                     .SingleOrDefault();

            if (course != null)
            {
                viewModels = session.Query<Student>()
                                    .Where(s => s.Id.In(course.StudentsIds))
                                    .Select(s => new StudentViewModel()
                                    {
                                        Email = s.Email,
                                        Id = s.Id,
                                        Name = s.Name,
                                        Password = s.Password,
                                        RegistrationCode = s.RegistrationCode
                                    })
                                    .OrderBy(s => s.Name)
                                    .ToList<StudentViewModel>();
            }

            return viewModels;
        }
    }
}