namespace SchoolLineup.Web.Mvc.Controllers.Queries.School
{
    using Raven.Client;
    using SchoolLineup.Domain.Entities;
    using SchoolLineup.Web.Mvc.Controllers.ViewModels;
    using System.Collections.Generic;
    using System.Linq;

    public class SchoolListQuery : ISchoolListQuery
    {
        private readonly IDocumentSession session;

        public SchoolListQuery(IDocumentSession session)
        {
            this.session = session;
        }

        public IList<SchoolViewModel> GetAll()
        {
            return session.Query<School>()
                          .Select(school => new SchoolViewModel()
                          {
                              Id = school.Id,
                              Name = school.Name,
                              Email = school.Email,
                              Phone = school.Phone,
                              ManagerName = school.ManagerName
                          })
                          .OrderBy(school => school.Name)
                          .ToList<SchoolViewModel>();
        }

        public School Get(int id)
        {
            return session.Load<School>(id);
        }

        public IList<School> GetAllEntities()
        {
            return session.Query<School>()
                          .ToList();
        }
    }
}