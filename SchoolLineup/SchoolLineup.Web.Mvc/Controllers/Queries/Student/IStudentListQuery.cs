namespace SchoolLineup.Web.Mvc.Controllers.Queries.Student
{
    using SchoolLineup.Domain.Entities;
    using SchoolLineup.Web.Mvc.Controllers.ViewModels;
    using System.Collections.Generic;

    public interface IStudentListQuery
    {
        Student Get(string email, string password);
        Student Get(int id);
        IEnumerable<StudentViewModel> GetAll();
        IEnumerable<StudentViewModel> GetAll(string query);
        IEnumerable<StudentViewModel> GetAllByCourse(int courseId);
    }
}