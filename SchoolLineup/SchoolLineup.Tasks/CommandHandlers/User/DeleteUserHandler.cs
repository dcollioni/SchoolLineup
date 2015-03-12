namespace UserLineup.Tasks.CommandHandlers.User
{
    using SchoolLineup.Domain.Contracts.Repositories;
    using SchoolLineup.Tasks.Commands.User;
    using SharpArch.Domain.Commands;

    public class DeleteUserHandler : ICommandHandler<DeleteUserCommand>
    {
        private readonly IUserRepository userRepository;

        public DeleteUserHandler(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public void Handle(DeleteUserCommand command)
        {
            if (command.IsValid())
            {
                userRepository.Delete(command.EntityId);
                command.Success = true;
            }
        }
    }
}