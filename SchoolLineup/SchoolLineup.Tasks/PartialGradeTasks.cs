namespace CollegeLineup.Tasks
{
    using SchoolLineup.Domain.Contracts.Repositories;
    using SchoolLineup.Domain.Contracts.Tasks;
    using SchoolLineup.Domain.Entities;

    public class PartialGradeTasks : IPartialGradeTasks
    {
        private readonly IPartialGradeRepository partialGradeRepository;
        private readonly IExamRepository examRepository;

        public PartialGradeTasks(IPartialGradeRepository partialGradeRepository,
                                 IExamRepository examRepository)
        {
            this.partialGradeRepository = partialGradeRepository;
            this.examRepository = examRepository;
        }

        public bool IsNameUnique(PartialGrade entity)
        {
            return partialGradeRepository.CountByName(entity) == 0;
        }

        public bool IsOrderUnique(PartialGrade entity)
        {
            return partialGradeRepository.CountByOrder(entity) == 0;
        }

        public bool HasChildren(int id)
        {
            var childrenCount = 0;

            childrenCount += examRepository.CountByPartialGrade(id);

            return childrenCount > 0;
        }
    }
}