namespace SchoolLineup.Web.Mvc.Controllers.Queries.Teacher
{
    using Raven.Client;
    using Raven.Client.Linq;
    using SchoolLineup.Domain.Entities;
    using SchoolLineup.Web.Mvc.Controllers.ViewModels;
    using System.Collections.Generic;
    using System.Linq;

    public class TeacherListQuery : ITeacherListQuery
    {
        private readonly IDocumentSession session;

        public TeacherListQuery(IDocumentSession session)
        {
            this.session = session;
        }

        public Teacher Get(int id)
        {
            return session.Load<Teacher>(id);
        }

        public IEnumerable<TeacherViewModel> GetAll()
        {
            return session.Query<Teacher>()
                          .Select(t => new TeacherViewModel()
                          {
                              Email = t.Email,
                              Id = t.Id,
                              Name = t.Name
                          })
                          .OrderBy(t => t.Name)
                          .ToList<TeacherViewModel>();
        }
    }
}