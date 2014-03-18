namespace SchoolLineup.Tasks.CommandHandlers.School
{
    using SchoolLineup.Domain.Contracts.Repositories;
    using SchoolLineup.Tasks.Commands.School;
    using SharpArch.Domain.Commands;

    public class SaveSchoolHandler : ICommandHandler<SaveSchoolCommand>
    {
        private readonly ISchoolRepository schoolRepository;

        public SaveSchoolHandler(ISchoolRepository schoolRepository)
        {
            this.schoolRepository = schoolRepository;
        }

        public void Handle(SaveSchoolCommand command)
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