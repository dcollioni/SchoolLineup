namespace SchoolLineup.Infrastructure.Repositories
{
    using Raven.Client;
    using SchoolLineup.Domain.Contracts.Repositories;
    using SchoolLineup.Domain.Entities;
    using System.Linq;

    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(IDocumentSession session)
            : base(session)
        {
        }

        public User Get(string email, string password)
        {
            return Session.Query<User>()
                          .Where(u => u.Email == email && u.Password == password)
                          .SingleOrDefault();
        }
    }
}