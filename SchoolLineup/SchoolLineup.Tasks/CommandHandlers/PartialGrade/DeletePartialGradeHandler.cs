namespace PartialGradeLineup.Tasks.CommandHandlers.PartialGrade
{
    using SchoolLineup.Domain.Contracts.Repositories;
    using SchoolLineup.Tasks.Commands.PartialGrade;
    using SharpArch.Domain.Commands;

    public class DeletePartialGradeHandler : ICommandHandler<DeletePartialGradeCommand>
    {
        private readonly IPartialGradeRepository partialGradeRepository;

        public DeletePartialGradeHandler(IPartialGradeRepository partialGradeRepository)
        {
            this.partialGradeRepository = partialGradeRepository;
        }

        public void Handle(DeletePartialGradeCommand command)
        {
            if (command.IsValid())
            {
                partialGradeRepository.Delete(command.EntityId);
                command.Success = true;
            }
        }
    }
}