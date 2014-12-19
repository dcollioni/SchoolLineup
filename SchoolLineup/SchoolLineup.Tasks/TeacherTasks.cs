namespace StudentLineup.Tasks
{
    using SchoolLineup.Domain.Contracts.Repositories;
    using SchoolLineup.Domain.Contracts.Tasks;
    using SchoolLineup.Domain.Entities;
    using System.Collections.Generic;

    public class TeacherTasks : ITeacherTasks
    {
        private readonly ITeacherRepository teacherRepository;
        private readonly ICourseRepository courseRepository;

        public TeacherTasks(ITeacherRepository teacherRepository,
                            ICourseRepository courseRepository)
        {
            this.teacherRepository = teacherRepository;
            this.courseRepository = courseRepository;
        }

        public bool IsEmailUnique(Teacher entity)
        {
            return teacherRepository.CountByEmail(entity) == 0;
        }

        public bool HasChildren(int id)
        {
            var childrenCount = 0;

            childrenCount += courseRepository.CountByTeacher(id);

            return childrenCount > 0;
        }
    }
}