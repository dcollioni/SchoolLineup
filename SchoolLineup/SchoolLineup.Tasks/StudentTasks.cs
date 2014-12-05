namespace StudentLineup.Tasks
{
    using SchoolLineup.Domain.Contracts.Repositories;
    using SchoolLineup.Domain.Contracts.Tasks;
    using SchoolLineup.Domain.Entities;
    using System.Collections.Generic;

    public class StudentTasks : IStudentTasks
    {
        private readonly IStudentRepository studentRepository;
        private readonly IExamResultRepository examResultRepository;

        public StudentTasks(IStudentRepository studentRepository,
                            IExamResultRepository examResultRepository)
        {
            this.studentRepository = studentRepository;
            this.examResultRepository = examResultRepository;
        }

        public bool IsEmailUnique(Student entity)
        {
            return studentRepository.CountByEmail(entity) == 0;
        }

        public bool AreEmailsUnique(IEnumerable<string> emails)
        {
            return studentRepository.CountByEmailList(emails) == 0;
        }

        public bool HasChildren(int id)
        {
            var childrenCount = 0;

            childrenCount += examResultRepository.CountByStudent(id);

            return childrenCount > 0;
        }
    }
}