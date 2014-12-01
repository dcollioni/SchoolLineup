namespace SchoolLineup.Web.Mvc.Controllers.Queries.College
{
    using Raven.Client;
    using SchoolLineup.Domain.Entities;
    using SchoolLineup.Web.Mvc.Controllers.ViewModels;
    using System.Collections.Generic;
    using System.Linq;

    public class CollegeListQuery : ICollegeListQuery
    {
        private readonly IDocumentSession session;

        public CollegeListQuery(IDocumentSession session)
        {
            this.session = session;
        }

        public IList<CollegeViewModel> GetAll()
        {
            return session.Query<College>()
                          .Select(e => new CollegeViewModel()
                          {
                              Id = e.Id,
                              Name = e.Name
                          })
                          .OrderBy(e => e.Name)
                          .ToList<CollegeViewModel>();
        }

        public College Get(int id)
        {
            return session.Load<College>(id);
        }
    }
}