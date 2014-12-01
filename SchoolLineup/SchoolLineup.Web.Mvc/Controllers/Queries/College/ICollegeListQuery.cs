namespace SchoolLineup.Web.Mvc.Controllers.Queries.College
{
    using SchoolLineup.Domain.Entities;
    using SchoolLineup.Web.Mvc.Controllers.ViewModels;
    using System.Collections.Generic;

    public interface ICollegeListQuery
    {
        IList<CollegeViewModel> GetAll();
        College Get(int id);
    }
}