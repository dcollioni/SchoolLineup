namespace SchoolLineup.Infrastructure.Repositories
{
    using Raven.Client;
    using SchoolLineup.Domain.Contracts.Repositories;
    using SchoolLineup.Domain.Entities;

    public class SchoolRepository : BaseRepository<School>, ISchoolRepository
    {
        public SchoolRepository(IDocumentSession session)
            : base(session)
        {
        }
    }
}