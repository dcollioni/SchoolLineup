namespace CollegeLineup.Tasks
{
    using SchoolLineup.Domain.Contracts.Repositories;
    using SchoolLineup.Domain.Contracts.Tasks;
    using SchoolLineup.Domain.Entities;

    public class CollegeTasks : ICollegeTasks
    {
        private readonly ICollegeRepository collegeRepository;
        private readonly ICourseRepository courseRepository;

        public CollegeTasks(ICollegeRepository collegeRepository,
                            ICourseRepository courseRepository)
        {
            this.collegeRepository = collegeRepository;
            this.courseRepository = courseRepository;
        }

        public bool IsNameUnique(College entity)
        {
            return collegeRepository.CountByName(entity) == 0;
        }

        public bool HasChildren(int id)
        {
            var childrenCount = 0;

            childrenCount += courseRepository.CountByCollege(id);

            return childrenCount > 0;
        }
    }
}