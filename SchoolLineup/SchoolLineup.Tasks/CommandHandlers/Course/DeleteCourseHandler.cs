namespace CourseLineup.Tasks.CommandHandlers.Course
{
    using SchoolLineup.Domain.Contracts.Repositories;
    using SchoolLineup.Tasks.Commands.Course;
    using SharpArch.Domain.Commands;

    public class DeleteCourseHandler : ICommandHandler<DeleteCourseCommand>
    {
        private readonly ICourseRepository courseRepository;

        public DeleteCourseHandler(ICourseRepository courseRepository)
        {
            this.courseRepository = courseRepository;
        }

        public void Handle(DeleteCourseCommand command)
        {
            if (command.IsValid())
            {
                courseRepository.Delete(command.EntityId);
                command.Success = true;
            }
        }
    }
}