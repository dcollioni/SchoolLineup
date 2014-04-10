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
                    ViewBag.SchoolId = school.Id;

                    return View();
                }
            }

            return RedirectToAction("Index", "School");
        }

        public JsonResult GetAll(int id)
        {
            return Json(null, JsonRequestBehavior.AllowGet);
        }

        [Transaction]
        public JsonResult Save(SchoolViewModel viewModel)
        {
            //var entity = GetEntity(viewModel);

            //var command = new SaveSchoolCommand(entity, schoolTasks);

            //this.commandProcessor.Process(command);

            //if (!command.Success)
            //{
            //    return Json(new { Success = false, Messages = command.ValidationResults() });
            //}

            //viewModel = GetViewModel(command.Entity);
            //return Json(new { Success = true, Data = viewModel });

            return Json(null);
        }
    }
}