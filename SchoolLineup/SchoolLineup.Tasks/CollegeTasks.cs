namespace CollegeLineup.Tasks
{
    using SchoolLineup.Domain.Contracts.Repositories;
    using SchoolLineup.Domain.Contracts.Tasks;
    using SchoolLineup.Domain.Entities;

    public class CollegeTasks : ICollegeTasks
    {
        private readonly ICollegeRepository collegeRepository;

        public CollegeTasks(ICollegeRepository collegeRepository)
        {
            this.collegeRepository = collegeRepository;
        }

        public bool IsNameUnique(College entity)
        {
            return collegeRepository.CountByName(entity) == 0;
        }
    }
}