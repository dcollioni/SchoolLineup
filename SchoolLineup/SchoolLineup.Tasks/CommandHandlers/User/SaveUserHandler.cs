namespace SchoolLineup.Tasks.CommandHandlers.User
{
    using SchoolLineup.Domain.Contracts.Repositories;
    using SchoolLineup.Domain.Entities;
    using SchoolLineup.Tasks.Commands.User;
    using SchoolLineup.Util;
    using SharpArch.Domain.Commands;
    using System;

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
                ApplySavingRules(command.Entity);

                userRepository.SaveOrUpdate(command.Entity);
                command.Success = true;
            }
            else
            {
                userRepository.Evict(command.Entity);
            }
        }

        private void ApplySavingRules(User entity)
        {
            if (entity.Id == 0)
            {
                entity.Password = MD5Helper.GetHash("123456");
                entity.IsPasswordTemp = true;
                entity.CreatedOn = DateTime.Now;
            }
            else
            {
                entity.UpdatedOn = DateTime.Now;
            }
        }
    }
}