namespace SchoolLineup.Tasks.CommandHandlers.Account
{
    using SchoolLineup.Domain.Contracts.Repositories;
    using SchoolLineup.Tasks.Commands.Account;
    using SharpArch.Domain.Commands;
    using System;

    public class ChangePasswordHandler : ICommandHandler<ChangePasswordCommand>
    {
        private readonly IUserRepository userRepository;

        public ChangePasswordHandler(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public void Handle(ChangePasswordCommand command)
        {
            if (command.IsValid())
            {
                var user = userRepository.Get(command.UserId);
                user.Password = command.Password;
                user.IsPasswordTemp = false;
                user.UpdatedOn = DateTime.Now;

                userRepository.SaveOrUpdate(user);
                command.Success = true;
            }
        }
    }
}