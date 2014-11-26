namespace SchoolLineup.Web.Mvc.Controllers.Queries.PartialGrade
{
    using Raven.Client;
    using Raven.Client.Linq;
    using SchoolLineup.Domain.Entities;
    using System.Collections.Generic;
    using System.Linq;

    public class PartialGradeListQuery : IPartialGradeListQuery
    {
        private readonly IDocumentSession session;

        public PartialGradeListQuery(IDocumentSession session)
        {
            this.session = session;
        }

        public IEnumerable<PartialGrade> GetAll(int courseId)
        {
            return session.Query<PartialGrade>()
                          .Where(p => p.CourseId == courseId)
                          .OrderBy(p => p.Order)
                          .ToList();
        }
    }
}