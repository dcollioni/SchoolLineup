namespace SchoolLineup.Web.Mvc.Controllers.Queries.Teacher
{
    using SchoolLineup.Web.Mvc.Controllers.ViewModels;
    using System.Collections.Generic;

    public interface ITeacherListQuery
    {
        IEnumerable<TeacherViewModel> GetAll();
    }
}