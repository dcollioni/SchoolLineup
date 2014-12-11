namespace SchoolLineup.Tasks.CommandHandlers.PartialGrade
{
    using SchoolLineup.Domain.Contracts.Repositories;
    using SchoolLineup.Tasks.Commands.PartialGrade;
    using SharpArch.Domain.Commands;

    public class SavePartialGradeHandler : ICommandHandler<SavePartialGradeCommand>
    {
        private readonly IPartialGradeRepository partialGradeRepository;

        public SavePartialGradeHandler(IPartialGradeRepository partialGradeRepository)
        {
            this.partialGradeRepository = partialGradeRepository;
        }

        public void Handle(SavePartialGradeCommand command)
        {
            if (command.IsValid())
            {
                partialGradeRepository.SaveOrUpdate(command.Entity);
                command.Success = true;
            }
            else
            {
                partialGradeRepository.Evict(command.Entity);
            }
        }
    }
}