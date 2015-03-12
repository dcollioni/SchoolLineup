namespace SchoolLineup.Domain.Contracts.Tasks
{
    using SchoolLineup.Domain.Entities;

    public interface IUserTasks
    {
        User Get(string email, string password);
        bool IsEmailUnique(User entity);
    }
}