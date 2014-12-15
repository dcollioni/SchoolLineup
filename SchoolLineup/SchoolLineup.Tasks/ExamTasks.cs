namespace CollegeLineup.Tasks
{
    using SchoolLineup.Domain.Contracts.Repositories;
    using SchoolLineup.Domain.Contracts.Tasks;
    using SchoolLineup.Domain.Entities;

    public class ExamTasks : IExamTasks
    {
        private readonly IExamRepository examRepository;
        private readonly IExamResultRepository examResultRepository;

        public ExamTasks(IExamRepository examRepository,
                         IExamResultRepository examResultRepository)
        {
            this.examRepository = examRepository;
            this.examResultRepository = examResultRepository;
        }

        public bool IsNameUnique(Exam entity)
        {
            return examRepository.CountByName(entity) == 0;
        }

        public bool HasChildren(int id)
        {
            var childrenCount = 0;

            childrenCount += examResultRepository.CountByExam(id);

            return childrenCount > 0;
        }

        public Exam Get(int id)
        {
            return examRepository.Get(id);
        }
    }
}