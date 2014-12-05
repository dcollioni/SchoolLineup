namespace SchoolLineup.Infrastructure.Repositories
{
    using Raven.Client;
    using SchoolLineup.Domain.Contracts.Repositories;
    using SchoolLineup.Domain.Entities;
    using System.Linq;

    public class ExamResultRepository : BaseRepository<ExamResult>, IExamResultRepository
    {
        public ExamResultRepository(IDocumentSession session)
            : base(session)
        {
        }

        public int CountByStudent(int studentId)
        {
            return Session.Query<ExamResult>()
                          .Where(e => e.StudentId == studentId)
                          .Count();
        }
    }
}