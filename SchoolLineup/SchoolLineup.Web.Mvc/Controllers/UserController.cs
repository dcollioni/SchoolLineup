namespace SchoolLineup.Web.Mvc.Controllers
{
    using SchoolLineup.Domain.Contracts.Tasks;
    using SchoolLineup.Domain.Entities;
    using SchoolLineup.Tasks.Commands.User;
    using SchoolLineup.Web.Mvc.ActionFilters;
    using SchoolLineup.Web.Mvc.Controllers.Queries.User;
    using SchoolLineup.Web.Mvc.Controllers.ViewModels;
    using SharpArch.Domain.Commands;
    using SharpArch.RavenDb.Web.Mvc;
    using System.Web.Mvc;

    [RequiresAuthentication]
    public class UserController : BaseController
    {
        private readonly ICommandProcessor commandProcessor;
        private readonly IUserTasks userTasks;
        private readonly IUserListQuery userListQuery;

        public UserController(ICommandProcessor commandProcessor,
                              IUserTasks userTasks,
                              IUserListQuery userListQuery)
        {
            this.commandProcessor = commandProcessor;
            this.userTasks = userTasks;
            this.userListQuery = userListQuery;
        }

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetAll()
        {
            var data = userListQuery.GetAll();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [Transaction]
        public JsonResult Save(UserViewModel viewModel)
        {
            var entity = GetEntity(viewModel);

            var command = new SaveUserCommand(entity, userTasks);

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
            var command = new DeleteUserCommand(id, userTasks);

            this.commandProcessor.Process(command);

            if (!command.Success)
            {
                return Json(new { Success = false, Messages = command.ValidationResults() });
            }

            return Json(new { Success = true });
        }

        public JsonResult IsEmailUnique(UserViewModel viewModel)
        {
            var entity = GetEntity(viewModel);
            var isUnique = userTasks.IsEmailUnique(entity);

            return Json(isUnique, JsonRequestBehavior.AllowGet);
        }

        private User GetEntity(UserViewModel viewModel)
        {
            var entity = new User();

            if (viewModel.Id > 0)
            {
                entity = userListQuery.Get(viewModel.Id);
            }

            entity.Name = GetTrimOrNull(viewModel.Name);
            entity.Email = GetTrimOrNull(viewModel.Email);
            entity.Profile = viewModel.Profile;

            return entity;
        }

        private UserViewModel GetViewModel(User entity)
        {
            var viewModel = new UserViewModel();

            viewModel.Id = entity.Id;
            viewModel.Name = entity.Name;
            viewModel.Email = entity.Email;
            viewModel.Profile = entity.Profile;

            return viewModel;
        }
    }
}