namespace SchoolLineup.Domain.Contracts.Repositories
{
    using SchoolLineup.Domain.Entities;
    using SharpArch.Domain.PersistenceSupport;

    public interface IUserRepository : IRepository<User>
    {
        void Evict(User entity);
        void Delete(int entityId);
        User Get(string email, string password);
        int CountByEmail(User entity);
    }
}