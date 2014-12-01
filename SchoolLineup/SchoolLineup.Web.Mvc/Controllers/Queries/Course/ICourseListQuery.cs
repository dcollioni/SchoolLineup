namespace SchoolLineup.Web.Mvc.Controllers.Queries.Course
{
    using SchoolLineup.Domain.Entities;
    using SchoolLineup.Web.Mvc.Controllers.ViewModels;
    using System.Collections.Generic;

    public interface ICourseListQuery
    {
        IEnumerable<CourseViewModel> GetAll(int studentId);
        IList<CourseViewModel> GetAllByCollege(int collegeId);
        Course Get(int id);
    }
}