namespace SchoolLineup.Tasks.CommandHandlers.School
{
    using SchoolLineup.Domain.Contracts.Repositories;
    using SchoolLineup.Tasks.Commands.School;
    using SharpArch.Domain.Commands;

    public class CreateSchoolHandler : ICommandHandler<CreateSchoolCommand>
    {
        private readonly ISchoolRepository schoolRepository;

        public CreateSchoolHandler(ISchoolRepository schoolRepository)
        {
            this.schoolRepository = schoolRepository;
        }

        public void Handle(CreateSchoolCommand command)
        {
            if (command.IsValid())
            {
                schoolRepository.SaveOrUpdate(command.Entity);
                command.Success = true;
            }
            else
            {
                schoolRepository.Evict(command.Entity);
            }
        }
    }
}