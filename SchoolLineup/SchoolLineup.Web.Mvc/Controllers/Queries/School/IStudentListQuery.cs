namespace SchoolLineup.Web.Mvc.Controllers.Queries.School
{
    using SchoolLineup.Domain.Entities;

    public interface IStudentListQuery
    {
        Student Get(string email, string password);
    }
}