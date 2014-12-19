namespace SchoolLineup.Infrastructure.Repositories
{
    using Raven.Client;
    using SchoolLineup.Domain.Contracts.Repositories;
    using SchoolLineup.Domain.Entities;
    using System;
    using System.Linq;

    public class TeacherRepository : BaseRepository<Teacher>, ITeacherRepository
    {
        public TeacherRepository(IDocumentSession session)
            : base(session)
        {
        }

        public int CountByEmail(Teacher entity)
        {
            return Session.Query<Teacher>()
                          .Count(e => e.Email.Equals(entity.Email, StringComparison.OrdinalIgnoreCase)
                                   && e.Id != entity.Id);
        }
    }
}