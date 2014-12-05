namespace SchoolLineup.Infrastructure.Repositories
{
    using Raven.Client;
    using SchoolLineup.Domain.Contracts.Repositories;
    using SchoolLineup.Domain.Entities;
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
    }
}