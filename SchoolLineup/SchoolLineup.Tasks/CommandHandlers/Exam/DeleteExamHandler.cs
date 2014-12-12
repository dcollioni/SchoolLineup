namespace ExamLineup.Tasks.CommandHandlers.Exam
{
    using SchoolLineup.Domain.Contracts.Repositories;
    using SchoolLineup.Tasks.Commands.Exam;
    using SharpArch.Domain.Commands;

    public class DeleteExamHandler : ICommandHandler<DeleteExamCommand>
    {
        private readonly IExamRepository examRepository;

        public DeleteExamHandler(IExamRepository examRepository)
        {
            this.examRepository = examRepository;
        }

        public void Handle(DeleteExamCommand command)
        {
            if (command.IsValid())
            {
                examRepository.Delete(command.EntityId);
                command.Success = true;
            }
        }
    }
}