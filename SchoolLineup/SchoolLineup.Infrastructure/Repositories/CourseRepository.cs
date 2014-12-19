namespace SchoolLineup.Infrastructure.Repositories
{
    using Raven.Client;
    using SchoolLineup.Domain.Contracts.Repositories;
    using SchoolLineup.Domain.Entities;
    using System.Linq;

    public class CourseRepository : BaseRepository<Course>, ICourseRepository
    {
        public CourseRepository(IDocumentSession session)
            : base(session)
        {
        }

        public int CountByCollege(int collegeId)
        {
            return Session.Query<Course>()
                          .Where(e => e.CollegeId == collegeId)
                          .Count();
        }

        public int CountByTeacher(int teacherId)
        {
            return Session.Query<Course>()
                          .Where(e => e.TeacherId == teacherId)
                          .Count();
        }
    }
}