namespace SchoolLineup.Web.Mvc.Controllers.Queries.User
{
    using Raven.Client;
    using Raven.Client.Linq;
    using SchoolLineup.Domain.Entities;
    using SchoolLineup.Web.Mvc.Controllers.ViewModels;
    using System.Collections.Generic;
    using System.Linq;

    public class UserListQuery : IUserListQuery
    {
        private readonly IDocumentSession session;

        public UserListQuery(IDocumentSession session)
        {
            this.session = session;
        }

        public User Get(int id)
        {
            return session.Load<User>(id);
        }

        public IEnumerable<UserViewModel> GetAll()
        {
            return session.Query<User>()
                          .Select(t => new UserViewModel()
                          {
                              Email = t.Email,
                              Id = t.Id,
                              Name = t.Name,
                              Profile = t.Profile
                          })
                          .OrderBy(t => t.Name)
                          .ToList<UserViewModel>();
        }
    }
}