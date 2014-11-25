namespace SchoolLineup.Web.Mvc.Controllers.Queries.Course
{
    using SchoolLineup.Web.Mvc.Controllers.ViewModels;
    using System.Collections.Generic;

    public interface ICourseListQuery
    {
        IEnumerable<CourseViewModel> GetAll(int studentId);
    }
}