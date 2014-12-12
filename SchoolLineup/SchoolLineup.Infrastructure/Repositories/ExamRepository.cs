namespace SchoolLineup.Infrastructure.Repositories
{
    using Raven.Client;
    using SchoolLineup.Domain.Contracts.Repositories;
    using SchoolLineup.Domain.Entities;
    using System;
    using System.Linq;

    public class ExamRepository : BaseRepository<Exam>, IExamRepository
    {
        public ExamRepository(IDocumentSession session)
            : base(session)
        {
        }

        public int CountByPartialGrade(int partialGradeId)
        {
            return Session.Query<Exam>()
                          .Where(e => e.PartialGradeId == partialGradeId)
                          .Count();
        }

        public int CountByName(Exam entity)
        {
            return Session.Query<Exam>()
                          .Count(e => e.Name.Equals(entity.Name, StringComparison.OrdinalIgnoreCase)
                                   && e.PartialGradeId == entity.PartialGradeId
                                   && e.Id != entity.Id);
        }
    }
}