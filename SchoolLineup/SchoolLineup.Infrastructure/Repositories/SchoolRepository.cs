namespace SchoolLineup.Infrastructure.Repositories
{
    using Raven.Client;
    using SchoolLineup.Domain.Contracts.Repositories;
    using SchoolLineup.Domain.Entities;
    using System;
    using System.Linq;

    public class SchoolRepository : BaseRepository<School>, ISchoolRepository
    {
        public SchoolRepository(IDocumentSession session)
            : base(session)
        {
        }

        public int CountByName(School entity)
        {
            return Session.Query<School>()
                          .Count(school => school.Name.Equals(entity.Name, StringComparison.OrdinalIgnoreCase)
                                           && school.Id != entity.Id);
        }
    }
}