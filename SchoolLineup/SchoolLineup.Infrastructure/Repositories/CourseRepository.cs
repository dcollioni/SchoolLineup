namespace SchoolLineup.Infrastructure.Repositories
{
    using Raven.Client;
    using SchoolLineup.Domain.Contracts.Repositories;
    using SchoolLineup.Domain.Entities;

    public class CourseRepository : BaseRepository<Course>, ICourseRepository
    {
        public CourseRepository(IDocumentSession session)
            : base(session)
        {
        }
    }
}