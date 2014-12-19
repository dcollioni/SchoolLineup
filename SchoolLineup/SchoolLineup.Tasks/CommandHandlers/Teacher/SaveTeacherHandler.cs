namespace SchoolLineup.Tasks.CommandHandlers.Teacher
{
    using SchoolLineup.Domain.Contracts.Repositories;
    using SchoolLineup.Tasks.Commands.Teacher;
    using SharpArch.Domain.Commands;

    public class SaveTeacherHandler : ICommandHandler<SaveTeacherCommand>
    {
        private readonly ITeacherRepository teacherRepository;

        public SaveTeacherHandler(ITeacherRepository teacherRepository)
        {
            this.teacherRepository = teacherRepository;
        }

        public void Handle(SaveTeacherCommand command)
        {
            if (command.IsValid())
            {
                teacherRepository.SaveOrUpdate(command.Entity);
                command.Success = true;
            }
            else
            {
                teacherRepository.Evict(command.Entity);
            }
        }
    }
}