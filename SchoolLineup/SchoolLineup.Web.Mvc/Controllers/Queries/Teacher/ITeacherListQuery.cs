namespace SchoolLineup.Web.Mvc.Controllers.Queries.Teacher
{
    using SchoolLineup.Domain.Entities;
    using SchoolLineup.Web.Mvc.Controllers.ViewModels;
    using System.Collections.Generic;

    public interface ITeacherListQuery
    {
        Teacher Get(int id);
        IEnumerable<TeacherViewModel> GetAll();
    }
}