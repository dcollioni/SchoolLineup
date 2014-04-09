namespace SchoolLineup.Web.Mvc.Controllers
{
    using SchoolLineup.Domain.Contracts.Tasks;
    using SchoolLineup.Domain.Entities;
    using SchoolLineup.Tasks.Commands.School;
    using SchoolLineup.Web.Mvc.Controllers.Queries.School;
    using SchoolLineup.Web.Mvc.Controllers.ViewModels;
    using SharpArch.Domain.Commands;
    using SharpArch.RavenDb.Web.Mvc;
    using System.Web.Mvc;

    public class CourseController : BaseController
    {
        private readonly ISchoolListQuery schoolListQuery;
        //private readonly ISchoolTasks schoolTasks;
        //private readonly ICommandProcessor commandProcessor;

        public CourseController(ISchoolListQuery schoolListQuery)
        {
            this.schoolListQuery = schoolListQuery;
            //this.schoolTasks = schoolTasks;
            //this.commandProcessor = commandProcessor;
        }

        public ActionResult Index(int? id)
        {
            if (id.HasValue)
            {
                var school = schoolListQuery.Get(id.Value);

                if (school != null)
                {
                    ViewBag.SchoolName = school.Name;

                    return View();
                }
            }

            return RedirectToAction("Index", "School");
        }
    }
}