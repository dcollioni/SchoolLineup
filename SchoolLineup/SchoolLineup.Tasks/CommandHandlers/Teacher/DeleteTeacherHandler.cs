namespace TeacherLineup.Tasks.CommandHandlers.Teacher
{
    using SchoolLineup.Domain.Contracts.Repositories;
    using SchoolLineup.Tasks.Commands.Teacher;
    using SharpArch.Domain.Commands;

    public class DeleteTeacherHandler : ICommandHandler<DeleteTeacherCommand>
    {
        private readonly ITeacherRepository teacherRepository;

        public DeleteTeacherHandler(ITeacherRepository teacherRepository)
        {
            this.teacherRepository = teacherRepository;
        }

        public void Handle(DeleteTeacherCommand command)
        {
            if (command.IsValid())
            {
                teacherRepository.Delete(command.EntityId);
                command.Success = true;
            }
        }
    }
}