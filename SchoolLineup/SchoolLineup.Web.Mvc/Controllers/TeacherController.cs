namespace SchoolLineup.Web.Mvc.Controllers
{
    using SchoolLineup.Domain.Contracts.Tasks;
    using SchoolLineup.Domain.Entities;
    using SchoolLineup.Domain.Enums;
    using SchoolLineup.Tasks.Commands.Teacher;
    using SchoolLineup.Web.Mvc.ActionFilters;
    using SchoolLineup.Web.Mvc.Controllers.Queries.Teacher;
    using SchoolLineup.Web.Mvc.Controllers.ViewModels;
    using SharpArch.Domain.Commands;
    using SharpArch.RavenDb.Web.Mvc;
    using System.Web.Mvc;

    [RequiresAuthentication(DeniedUserProfiles = new[] { UserProfile.Teacher })]
    public class TeacherController : BaseController
    {
        private readonly ICommandProcessor commandProcessor;
        private readonly ITeacherListQuery teacherListQuery;
        private readonly ITeacherTasks teacherTasks;

        public TeacherController(ICommandProcessor commandProcessor,
                                 ITeacherListQuery teacherListQuery,
                                 ITeacherTasks teacherTasks)
        {
            this.commandProcessor = commandProcessor;
            this.teacherListQuery = teacherListQuery;
            this.teacherTasks = teacherTasks;
        }

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetAll()
        {
            var data = teacherListQuery.GetAll();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [Transaction]
        public JsonResult Save(TeacherViewModel viewModel)
        {
            var entity = GetEntity(viewModel);

            var command = new SaveTeacherCommand(entity, teacherTasks);

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
            var command = new DeleteTeacherCommand(id, teacherTasks);

            this.commandProcessor.Process(command);

            if (!command.Success)
            {
                return Json(new { Success = false, Messages = command.ValidationResults() });
            }

            return Json(new { Success = true });
        }

        public JsonResult IsEmailUnique(TeacherViewModel viewModel)
        {
            var entity = GetEntity(viewModel);
            var isUnique = teacherTasks.IsEmailUnique(entity);

            return Json(isUnique, JsonRequestBehavior.AllowGet);
        }

        private Teacher GetEntity(TeacherViewModel viewModel)
        {
            var entity = new Teacher();

            if (viewModel.Id > 0)
            {
                entity = teacherListQuery.Get(viewModel.Id);
            }

            entity.Name = GetTrimOrNull(viewModel.Name);
            entity.Email = GetTrimOrNull(viewModel.Email);

            return entity;
        }

        private TeacherViewModel GetViewModel(Teacher entity)
        {
            var viewModel = new TeacherViewModel();

            viewModel.Id = entity.Id;
            viewModel.Name = entity.Name;
            viewModel.Email = entity.Email;

            return viewModel;
        }
    }
}