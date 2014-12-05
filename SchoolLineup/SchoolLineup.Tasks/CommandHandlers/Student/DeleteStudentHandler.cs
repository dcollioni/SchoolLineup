namespace StudentLineup.Tasks.CommandHandlers.Student
{
    using SchoolLineup.Domain.Contracts.Repositories;
    using SchoolLineup.Tasks.Commands.Student;
    using SharpArch.Domain.Commands;

    public class DeleteStudentHandler : ICommandHandler<DeleteStudentCommand>
    {
        private readonly IStudentRepository studentRepository;

        public DeleteStudentHandler(IStudentRepository studentRepository)
        {
            this.studentRepository = studentRepository;
        }

        public void Handle(DeleteStudentCommand command)
        {
            if (command.IsValid())
            {
                studentRepository.Delete(command.EntityId);
                command.Success = true;
            }
        }
    }
}