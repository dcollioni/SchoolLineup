namespace SchoolLineup.Web.Mvc.Controllers.Queries.Exam
{
    using Raven.Client;
    using Raven.Client.Linq;
    using SchoolLineup.Domain.Entities;
    using SchoolLineup.Web.Mvc.Controllers.ViewModels;
    using System.Collections.Generic;
    using System.Linq;

    public class ExamListQuery : IExamListQuery
    {
        private readonly IDocumentSession session;

        public ExamListQuery(IDocumentSession session)
        {
            this.session = session;
        }

        public Exam Get(int id)
        {
            return session.Load<Exam>(id);
        }

        public IEnumerable<ExamViewModel> GetAll(int partialGradeId)
        {
            return session.Query<Exam>()
                          .Where(e => e.PartialGradeId == partialGradeId)
                          .OrderBy(e => e.Date)
                          .Select(e => new ExamViewModel()
                          {
                              Date = e.Date,
                              Description = e.Description,
                              Id = e.Id,
                              Name = e.Name,
                              PartialGradeId = e.PartialGradeId,
                              Value = e.Value
                          })
                          .ToList();
        }
    }
}