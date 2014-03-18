namespace SchoolLineup.Web.Mvc.Controllers
{
    using SchoolLineup.Web.Mvc.Controllers.Queries.School;
    using System.Web.Mvc;

    public class SchoolController : Controller
    {
        private readonly ISchoolListQuery schoolListQuery;

        public SchoolController(ISchoolListQuery schoolListQuery)
        {
            this.schoolListQuery = schoolListQuery;
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
    }
}