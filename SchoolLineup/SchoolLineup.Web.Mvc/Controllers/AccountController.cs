namespace SchoolLineup.Web.Mvc.Controllers
{
    using SchoolLineup.Domain.Contracts.Tasks;
    using SchoolLineup.Tasks.Commands.Account;
    using SchoolLineup.Util;
    using SchoolLineup.Web.Mvc.ActionFilters;
    using SharpArch.Domain.Commands;
    using SharpArch.RavenDb.Web.Mvc;
    using System.Web.Mvc;

    public class AccountController : BaseController
    {
        private readonly IUserTasks userTasks;
        private readonly ICommandProcessor commandProcessor;

        public AccountController(IUserTasks userTasks,
                                 ICommandProcessor commandProcessor)
        {
            this.userTasks = userTasks;
            this.commandProcessor = commandProcessor;
        }

        public ActionResult Login()
        {
            if (UserLogged != null)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        public ActionResult Login(string email, string password)
        {
            password = MD5Helper.GetHash(password);

            var user = userTasks.Get(email, password);

            if (user == null)
            {
                ViewBag.Error = "Dados de acesso inválidos";
                return Redirect("/Login");
            }

            UserLogged = user;

            if (UserLogged.IsPasswordTemp)
            {
                return RedirectToAction("ChangePassword");
            }

            return RedirectToAction("Index", "Home");
        }

        [RequiresAuthentication]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [RequiresAuthentication]
        [HttpPost]
        [Transaction]
        public ActionResult ChangePassword(string password, string passwordConfirmation)
        {
            password = MD5Helper.GetHash(password);
            passwordConfirmation = MD5Helper.GetHash(passwordConfirmation);

            var changePasswordCommand = new ChangePasswordCommand(UserLogged.Id,
                                                                  password,
                                                                  passwordConfirmation);

            commandProcessor.Process(changePasswordCommand);

            if (!changePasswordCommand.Success)
            {
                ViewBag.Errors = changePasswordCommand.ValidationResults();
                return View();
            }

            return RedirectToAction("Index", "Home");
        }

        [RequiresAuthentication]
        public ActionResult Logout()
        {
            UserLogged = null;
            return RedirectToAction("Index", "Home");
        }
    }
}