namespace SchoolLineup.Tasks.CommandHandlers.College
{
    using SchoolLineup.Domain.Contracts.Repositories;
    using SchoolLineup.Tasks.Commands.College;
    using SharpArch.Domain.Commands;

    public class SaveCollegeHandler : ICommandHandler<SaveCollegeCommand>
    {
        private readonly ICollegeRepository collegeRepository;

        public SaveCollegeHandler(ICollegeRepository collegeRepository)
        {
            this.collegeRepository = collegeRepository;
        }

        public void Handle(SaveCollegeCommand command)
        {
            if (command.IsValid())
            {
                collegeRepository.SaveOrUpdate(command.Entity);
                command.Success = true;
            }
            else
            {
                collegeRepository.Evict(command.Entity);
            }
        }
    }
}