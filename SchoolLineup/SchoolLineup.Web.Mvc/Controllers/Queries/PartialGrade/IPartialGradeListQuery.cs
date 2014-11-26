namespace SchoolLineup.Web.Mvc.Controllers.Queries.PartialGrade
{
    using SchoolLineup.Domain.Entities;
    using System.Collections.Generic;

    public interface IPartialGradeListQuery
    {
        IEnumerable<PartialGrade> GetAll(int courseId);
    }
}