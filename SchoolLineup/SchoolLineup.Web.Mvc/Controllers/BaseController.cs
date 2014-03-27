namespace SchoolLineup.Web.Mvc.Controllers
{
    using System.Text.RegularExpressions;
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

        protected string GetPhoneNumber(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                value = Regex.Replace(value, @"\D", string.Empty); // Remove all non-digit chars.
            }

            return value;
        }
    }
}