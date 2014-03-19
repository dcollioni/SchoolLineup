namespace SchoolLineup.Tasks.CommandHandlers.School
{
    using SchoolLineup.Domain.Contracts.Repositories;
    using SchoolLineup.Tasks.Commands.School;
    using SharpArch.Domain.Commands;

    public class DeleteSchoolHandler : ICommandHandler<DeleteSchoolCommand>
    {
        private readonly ISchoolRepository schoolRepository;

        public DeleteSchoolHandler(ISchoolRepository schoolRepository)
        {
            this.schoolRepository = schoolRepository;
        }

        public void Handle(DeleteSchoolCommand command)
        {
            if (command.IsValid())
            {
                schoolRepository.Delete(command.EntityId);
                command.Success = true;
            }
        }
    }
}