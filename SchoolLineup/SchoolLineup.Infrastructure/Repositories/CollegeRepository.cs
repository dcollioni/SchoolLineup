namespace SchoolLineup.Infrastructure.Repositories
{
    using Raven.Client;
    using SchoolLineup.Domain.Contracts.Repositories;
    using SchoolLineup.Domain.Entities;
    using System;
    using System.Linq;

    public class CollegeRepository : BaseRepository<College>, ICollegeRepository
    {
        public CollegeRepository(IDocumentSession session)
            : base(session)
        {
        }

        public int CountByName(College entity)
        {
            return Session.Query<College>()
                          .Count(e => e.Name.Equals(entity.Name, StringComparison.OrdinalIgnoreCase)
                                           && e.Id != entity.Id);
        }
    }
}