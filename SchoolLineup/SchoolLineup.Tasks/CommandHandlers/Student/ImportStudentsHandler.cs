namespace SchoolLineup.Tasks.CommandHandlers.Student
{
    using SchoolLineup.Domain.Contracts.Repositories;
    using SchoolLineup.Tasks.Commands.Student;
    using SharpArch.Domain.Commands;

    public class ImportStudentsHandler : ICommandHandler<ImportStudentsCommand>
    {
        private readonly IStudentRepository studentRepository;

        public ImportStudentsHandler(IStudentRepository studentRepository)
        {
            this.studentRepository = studentRepository;
        }

        public void Handle(ImportStudentsCommand command)
        {
            if (command.IsValid())
            {
                foreach (var entity in command.Entities)
                {
                    studentRepository.SaveOrUpdate(entity);
                }

                command.Success = true;
            }
            else
            {
                foreach (var entity in command.Entities)
                {
                    studentRepository.Evict(entity);
                }
            }
        }
    }
}