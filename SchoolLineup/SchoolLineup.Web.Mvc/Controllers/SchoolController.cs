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

    public class SchoolController : BaseController
    {
        private readonly ISchoolListQuery schoolListQuery;
        private readonly ISchoolTasks schoolTasks;
        private readonly ICommandProcessor commandProcessor;

        public SchoolController(ISchoolListQuery schoolListQuery,
                                ISchoolTasks schoolTasks,
                                ICommandProcessor commandProcessor)
        {
            this.schoolListQuery = schoolListQuery;
            this.schoolTasks = schoolTasks;
            this.commandProcessor = commandProcessor;
        }

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetAll()
        {
            var schools = schoolListQuery.GetAll();

            return Json(schools, JsonRequestBehavior.AllowGet);
        }

        [Transaction]
        public JsonResult Save(SchoolViewModel viewModel)
        {
            var entity = GetEntity(viewModel);

            var command = new SaveSchoolCommand(entity, schoolTasks);

            this.commandProcessor.Process(command);

            if (!command.Success)
            {
                return Json(new { Success = false, Messages = command.ValidationResults() });
            }

            viewModel = GetViewModel(command.Entity);
            return Json(new { Success = true, Data = viewModel });
        }

        private School GetEntity(SchoolViewModel viewModel)
        {
            var entity = new School();

            if (viewModel.Id > 0)
            {
                entity = schoolListQuery.Get(viewModel.Id);
            }

            entity.Name = GetTrimOrNull(viewModel.Name);
            entity.Email = GetTrimOrNull(viewModel.Email);
            entity.ManagerName = GetTrimOrNull(viewModel.ManagerName);
            entity.Phone = GetTrimOrNull(viewModel.Phone);

            return entity;
        }

        private SchoolViewModel GetViewModel(School entity)
        {
            var viewModel = new SchoolViewModel();

            viewModel.Id = entity.Id;
            viewModel.Name = entity.Name;
            viewModel.Email = entity.Email;
            viewModel.ManagerName = entity.ManagerName;
            viewModel.Phone = entity.Phone;

            return viewModel;
        }
    }
}