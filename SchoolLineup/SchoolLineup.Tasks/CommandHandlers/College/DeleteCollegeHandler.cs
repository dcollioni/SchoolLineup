namespace CollegeLineup.Tasks.CommandHandlers.College
{
    using SchoolLineup.Domain.Contracts.Repositories;
    using SchoolLineup.Tasks.Commands.College;
    using SharpArch.Domain.Commands;

    public class DeleteCollegeHandler : ICommandHandler<DeleteCollegeCommand>
    {
        private readonly ICollegeRepository collegeRepository;

        public DeleteCollegeHandler(ICollegeRepository collegeRepository)
        {
            this.collegeRepository = collegeRepository;
        }

        public void Handle(DeleteCollegeCommand command)
        {
            if (command.IsValid())
            {
                collegeRepository.Delete(command.EntityId);
                command.Success = true;
            }
        }
    }
}