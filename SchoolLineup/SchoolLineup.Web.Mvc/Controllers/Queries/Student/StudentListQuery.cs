namespace SchoolLineup.Web.Mvc.Controllers.Queries.Student
{
    using Raven.Client;
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
            var md5Password = Encoding.UTF8.GetString(new MD5Cng().ComputeHash(Encoding.UTF8.GetBytes(password)));

            return session.Query<Student>()
                          .Where(s => s.Email == email && s.Password == md5Password)
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
    }
}