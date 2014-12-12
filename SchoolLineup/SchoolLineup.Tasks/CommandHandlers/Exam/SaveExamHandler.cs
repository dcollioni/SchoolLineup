namespace SchoolLineup.Tasks.CommandHandlers.Exam
{
    using SchoolLineup.Domain.Contracts.Repositories;
    using SchoolLineup.Tasks.Commands.Exam;
    using SharpArch.Domain.Commands;

    public class SaveExamHandler : ICommandHandler<SaveExamCommand>
    {
        private readonly IExamRepository examRepository;

        public SaveExamHandler(IExamRepository examRepository)
        {
            this.examRepository = examRepository;
        }

        public void Handle(SaveExamCommand command)
        {
            if (command.IsValid())
            {
                examRepository.SaveOrUpdate(command.Entity);
                command.Success = true;
            }
            else
            {
                examRepository.Evict(command.Entity);
            }
        }
    }
}