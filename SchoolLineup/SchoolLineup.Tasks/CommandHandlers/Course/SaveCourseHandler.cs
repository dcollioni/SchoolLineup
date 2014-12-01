namespace SchoolLineup.Tasks.CommandHandlers.Course
{
    using SchoolLineup.Domain.Contracts.Repositories;
    using SchoolLineup.Tasks.Commands.Course;
    using SharpArch.Domain.Commands;

    public class SaveCourseHandler : ICommandHandler<SaveCourseCommand>
    {
        private readonly ICourseRepository courseRepository;

        public SaveCourseHandler(ICourseRepository courseRepository)
        {
            this.courseRepository = courseRepository;
        }

        public void Handle(SaveCourseCommand command)
        {
            if (command.IsValid())
            {
                courseRepository.SaveOrUpdate(command.Entity);
                command.Success = true;
            }
            else
            {
                courseRepository.Evict(command.Entity);
            }
        }
    }
}