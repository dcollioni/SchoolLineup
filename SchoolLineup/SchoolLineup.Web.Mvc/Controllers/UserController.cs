namespace SchoolLineup.Web.Mvc.Controllers
{
    using System.Web.Mvc;

    public class UserController : Controller
    {
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string email, string password)
        {
            return View();
        }
    }
}
