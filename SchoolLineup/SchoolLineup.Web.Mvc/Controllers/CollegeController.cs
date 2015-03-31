namespace SchoolLineup.Web.Mvc.Controllers
{
    using SchoolLineup.Domain.Contracts.Tasks;
    using SchoolLineup.Domain.Entities;
    using SchoolLineup.Domain.Enums;
    using SchoolLineup.Tasks.Commands.College;
    using SchoolLineup.Web.Mvc.ActionFilters;
    using SchoolLineup.Web.Mvc.Controllers.Queries.College;
    using SchoolLineup.Web.Mvc.Controllers.ViewModels;
    using SharpArch.Domain.Commands;
    using SharpArch.RavenDb.Web.Mvc;
    using System.Web.Mvc;

    [RequiresAuthentication(DeniedUserProfiles = new[] { UserProfile.Teacher })]
    public class CollegeController : BaseController
    {
        private readonly ICommandProcessor commandProcessor;
        private readonly ICollegeListQuery collegeListQuery;
        private readonly ICollegeTasks collegeTasks;

        public CollegeController(ICommandProcessor commandProcessor,
                                 ICollegeListQuery collegeListQuery,
                                 ICollegeTasks collegeTasks)
        {
            this.commandProcessor = commandProcessor;
            this.collegeListQuery = collegeListQuery;
            this.collegeTasks = collegeTasks;
        }

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetAll()
        {
            var data = collegeListQuery.GetAll();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [Transaction]
        public JsonResult Save(CollegeViewModel viewModel)
        {
            var entity = GetEntity(viewModel);

            var command = new SaveCollegeCommand(entity, collegeTasks);

            this.commandProcessor.Process(command);

            if (!command.Success)
            {
                return Json(new { Success = false, Messages = command.ValidationResults() });
            }

            viewModel = GetViewModel(command.Entity);

            return Json(new { Success = true, Data = viewModel });
        }

        [Transaction]
        public JsonResult Delete(int id)
        {
            var command = new DeleteCollegeCommand(id, collegeTasks);

            this.commandProcessor.Process(command);

            if (!command.Success)
            {
                return Json(new { Success = false, Messages = command.ValidationResults() });
            }

            return Json(new { Success = true });
        }

        public JsonResult IsNameUnique(CollegeViewModel viewModel)
        {
            var entity = GetEntity(viewModel);
            var isNameUnique = collegeTasks.IsNameUnique(entity);

            return Json(isNameUnique, JsonRequestBehavior.AllowGet);
        }

        private College GetEntity(CollegeViewModel viewModel)
        {
            var entity = new College();

            if (viewModel.Id > 0)
            {
                entity = collegeListQuery.Get(viewModel.Id);
            }

            entity.Name = GetTrimOrNull(viewModel.Name);

            return entity;
        }

        private CollegeViewModel GetViewModel(College entity)
        {
            var viewModel = new CollegeViewModel();

            viewModel.Id = entity.Id;
            viewModel.Name = entity.Name;

            return viewModel;
        }
    }
}