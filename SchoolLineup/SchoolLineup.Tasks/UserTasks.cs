namespace StudentLineup.Tasks
{
    using SchoolLineup.Domain.Contracts.Repositories;
    using SchoolLineup.Domain.Contracts.Tasks;
    using SchoolLineup.Domain.Entities;

    public class UserTasks : IUserTasks
    {
        private readonly IUserRepository userRepository;

        public UserTasks(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public User Get(string email, string password)
        {
            return userRepository.Get(email, password);
        }

        public bool IsEmailUnique(User entity)
        {
            return userRepository.CountByEmail(entity) == 0;
        }
    }
}