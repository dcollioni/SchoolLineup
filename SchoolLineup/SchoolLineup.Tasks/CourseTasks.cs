namespace CollegeLineup.Tasks
{
    using SchoolLineup.Domain.Contracts.Repositories;
    using SchoolLineup.Domain.Contracts.Tasks;

    public class CourseTasks : ICourseTasks
    {
        private readonly ICourseRepository courseRepository;

        public CourseTasks(ICourseRepository courseRepository)
        {
            this.courseRepository = courseRepository;
        }
    }
}