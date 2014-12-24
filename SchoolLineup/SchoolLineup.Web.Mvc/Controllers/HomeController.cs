namespace SchoolLineup.Web.Mvc.Controllers
{
    using SchoolLineup.Web.Mvc.ActionFilters;
    using System.Web.Mvc;

    [RequiresAuthentication]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}