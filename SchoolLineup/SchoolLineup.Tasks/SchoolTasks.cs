namespace SchoolLineup.Tasks
{
    using SchoolLineup.Domain.Contracts.Repositories;
    using SchoolLineup.Domain.Contracts.Tasks;
    using SchoolLineup.Domain.Entities;

    public class SchoolTasks : ISchoolTasks
    {
        private readonly ISchoolRepository schoolRepository;

        public SchoolTasks(ISchoolRepository schoolRepository)
        {
            this.schoolRepository = schoolRepository;
        }

        public bool IsNameUnique(School entity)
        {
            return schoolRepository.CountByName(entity) == 0;
        }
    }
}