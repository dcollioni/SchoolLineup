namespace SchoolLineup.Infrastructure.Repositories
{
    using Raven.Client;
    using SchoolLineup.Domain.Contracts.Repositories;
    using SchoolLineup.Domain.Entities;

    public class ExamRepository : BaseRepository<Exam>, IExamRepository
    {
        public ExamRepository(IDocumentSession session)
            : base(session)
        {
        }
    }
}