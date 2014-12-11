namespace SchoolLineup.Web.Mvc.Controllers.Queries.PartialGrade
{
    using SchoolLineup.Domain.Entities;
    using SchoolLineup.Web.Mvc.Controllers.ViewModels;
    using System.Collections.Generic;

    public interface IPartialGradeListQuery
    {
        PartialGrade Get(int id);
        IEnumerable<PartialGrade> GetAll(int courseId);
        IEnumerable<PartialGradeViewModel> GetAllByCourse(int courseId);
    }
}