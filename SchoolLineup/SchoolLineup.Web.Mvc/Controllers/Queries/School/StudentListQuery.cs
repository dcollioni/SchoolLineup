namespace SchoolLineup.Web.Mvc.Controllers.Queries.School
{
    using Raven.Client;
    using SchoolLineup.Domain.Entities;
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
    }
}