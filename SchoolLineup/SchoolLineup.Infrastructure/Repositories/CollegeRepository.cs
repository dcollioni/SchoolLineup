namespace SchoolLineup.Infrastructure.Repositories
{
    using Raven.Client;
    using SchoolLineup.Domain.Contracts.Repositories;
    using SchoolLineup.Domain.Entities;

    public class CollegeRepository : BaseRepository<College>, ICollegeRepository
    {
        public CollegeRepository(IDocumentSession session)
            : base(session)
        {
        }
    }
}