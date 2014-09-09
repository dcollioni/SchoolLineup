namespace SchoolLineup.Web.Mvc.Controllers.Queries.School
{
    using SchoolLineup.Domain.Entities;
    using SchoolLineup.Web.Mvc.Controllers.ViewModels;
    using System.Collections.Generic;

    public interface ISchoolListQuery
    {
        IList<SchoolViewModel> GetAll();
        IList<School> GetAllEntities();
        School Get(int id);
    }
}