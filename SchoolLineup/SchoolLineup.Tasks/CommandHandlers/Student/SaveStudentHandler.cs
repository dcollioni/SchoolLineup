namespace SchoolLineup.Tasks.CommandHandlers.Student
{
    using SchoolLineup.Domain.Contracts.Repositories;
    using SchoolLineup.Tasks.Commands.Student;
    using SharpArch.Domain.Commands;

    public class SaveStudentHandler : ICommandHandler<SaveStudentCommand>
    {
        private readonly IStudentRepository studentRepository;

        public SaveStudentHandler(IStudentRepository studentRepository)
        {
            this.studentRepository = studentRepository;
        }

        public void Handle(SaveStudentCommand command)
        {
            if (command.IsValid())
            {
                studentRepository.SaveOrUpdate(command.Entity);
                command.Success = true;
            }
            else
            {
                studentRepository.Evict(command.Entity);
            }
        }
    }
}