namespace SchoolLineup.Web.Mvc.Controllers.Queries.School
{
    using SchoolLineup.Web.Mvc.Controllers.ViewModels;
    using System.Collections.Generic;

    public interface ISchoolListQuery
    {
        IList<SchoolViewModel> GetAll();
    }
}