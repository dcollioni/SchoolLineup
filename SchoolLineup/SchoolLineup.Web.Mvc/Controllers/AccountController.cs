namespace SchoolLineup.Web.Mvc.Controllers
{
    using SchoolLineup.Domain.Contracts.Tasks;
    using SchoolLineup.Util;
    using System.Web.Mvc;

    public class AccountController : BaseController
    {
        private readonly IUserTasks userTasks;

        public AccountController(IUserTasks userTasks)
        {
            this.userTasks = userTasks;
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

        public ActionResult ChangePassword()
        {
            return View();
        }
    }
}