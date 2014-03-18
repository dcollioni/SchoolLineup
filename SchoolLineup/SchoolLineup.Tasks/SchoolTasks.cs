namespace SchoolLineup.Tasks
{
    using SchoolLineup.Domain.Contracts.Repositories;
    using SchoolLineup.Domain.Contracts.Tasks;

    public class SchoolTasks : ISchoolTasks
    {
        private readonly ISchoolRepository schoolRepository;

        public SchoolTasks(ISchoolRepository schoolRepository)
        {
            this.schoolRepository = schoolRepository;
        }
    }
}