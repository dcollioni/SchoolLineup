namespace SchoolLineup.Web.Mvc.Controllers
{
    using System.Web.Mvc;

    public class BaseController : Controller
    {
        protected string GetTrimOrNull(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                value = value.Trim();
            }

            return value;
        }
    }
}