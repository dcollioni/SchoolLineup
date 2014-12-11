namespace SchoolLineup.Infrastructure.Repositories
{
    using Raven.Client;
    using SchoolLineup.Domain.Contracts.Repositories;
    using SchoolLineup.Domain.Entities;
    using System;
    using System.Linq;

    public class PartialGradeRepository : BaseRepository<PartialGrade>, IPartialGradeRepository
    {
        public PartialGradeRepository(IDocumentSession session)
            : base(session)
        {
        }

        public int CountByCourse(int courseId)
        {
            return Session.Query<PartialGrade>()
                          .Where(e => e.CourseId == courseId)
                          .Count();
        }

        public int CountByName(PartialGrade entity)
        {
            return Session.Query<PartialGrade>()
                          .Count(p => p.Name.Equals(entity.Name, StringComparison.OrdinalIgnoreCase)
                                   && p.CourseId == entity.CourseId
                                   && p.Id != entity.Id);
        }

        public int CountByOrder(PartialGrade entity)
        {
            return Session.Query<PartialGrade>()
                          .Count(p => p.Order == entity.Order
                                   && p.CourseId == entity.CourseId
                                   && p.Id != entity.Id);
        }
    }
}