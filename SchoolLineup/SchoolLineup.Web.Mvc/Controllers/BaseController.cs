namespace SchoolLineup.Web.Mvc.Controllers
{
    using SchoolLineup.Domain.Entities;
    using System.Text.RegularExpressions;
    using System.Web.Mvc;

    public class BaseController : Controller
    {
        protected User UserLogged
        {
            get
            {
                return Session["UserLogged"] as User;
            }
            set
            {
                Session["UserLogged"] = value;
            }
        }

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