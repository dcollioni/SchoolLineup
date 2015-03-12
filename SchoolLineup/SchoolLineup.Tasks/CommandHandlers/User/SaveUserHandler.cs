namespace SchoolLineup.Tasks.CommandHandlers.User
{
    using SchoolLineup.Domain.Contracts.Repositories;
    using SchoolLineup.Tasks.Commands.User;
    using SharpArch.Domain.Commands;

    public class SaveUserHandler : ICommandHandler<SaveUserCommand>
    {
        private readonly IUserRepository userRepository;

        public SaveUserHandler(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public void Handle(SaveUserCommand command)
        {
            if (command.IsValid())
            {
                userRepository.SaveOrUpdate(command.Entity);
                command.Success = true;
            }
            else
            {
                userRepository.Evict(command.Entity);
            }
        }
    }
}