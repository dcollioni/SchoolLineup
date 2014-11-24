namespace SchoolLineup.Infrastructure.Repositories
{
    using Raven.Client;
    using SchoolLineup.Domain.Contracts.Repositories;
    using SchoolLineup.Domain.Entities;

    public class PartialGradeRepository : BaseRepository<PartialGrade>, IPartialGradeRepository
    {
        public PartialGradeRepository(IDocumentSession session)
            : base(session)
        {
        }
    }
}