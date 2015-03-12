namespace SchoolLineup.Web.Mvc.Controllers.Queries.User
{
    using SchoolLineup.Domain.Entities;
    using SchoolLineup.Web.Mvc.Controllers.ViewModels;
    using System.Collections.Generic;

    public interface IUserListQuery
    {
        User Get(int id);
        IEnumerable<UserViewModel> GetAll();
    }
}