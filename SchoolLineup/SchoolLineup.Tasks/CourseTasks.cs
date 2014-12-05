namespace CollegeLineup.Tasks
{
    using SchoolLineup.Domain.Contracts.Repositories;
    using SchoolLineup.Domain.Contracts.Tasks;

    public class CourseTasks : ICourseTasks
    {
        private readonly ICourseRepository courseRepository;
        private readonly IPartialGradeRepository partialGradeRepository;

        public CourseTasks(ICourseRepository courseRepository,
                           IPartialGradeRepository partialGradeRepository)
        {
            this.courseRepository = courseRepository;
            this.partialGradeRepository = partialGradeRepository;
        }

        public bool HasChildren(int id)
        {
            var childrenCount = 0;

            childrenCount += partialGradeRepository.CountByCourse(id);

            return childrenCount > 0;
        }
    }
}