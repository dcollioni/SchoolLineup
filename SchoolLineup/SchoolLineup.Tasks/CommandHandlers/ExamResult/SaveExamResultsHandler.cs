namespace SchoolLineup.Tasks.CommandHandlers.ExamResult
{
    using SchoolLineup.Domain.Contracts.Repositories;
    using SchoolLineup.Tasks.Commands.ExamResult;
    using SharpArch.Domain.Commands;

    public class SaveExamResultsHandler : ICommandHandler<SaveExamResultsCommand>
    {
        private readonly IExamResultRepository examResultRepository;

        public SaveExamResultsHandler(IExamResultRepository studentRepository)
        {
            this.examResultRepository = studentRepository;
        }

        public void Handle(SaveExamResultsCommand command)
        {
            if (command.IsValid())
            {
                foreach (var entity in command.Entities)
                {
                    examResultRepository.SaveOrUpdate(entity);
                }

                command.Success = true;
            }
            else
            {
                foreach (var entity in command.Entities)
                {
                    examResultRepository.Evict(entity);
                }
            }
        }
    }
}