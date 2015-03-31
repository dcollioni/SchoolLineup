namespace SchoolLineup.Web.Mvc.ActionFilters
{
    using SchoolLineup.Domain.Entities;
    using SchoolLineup.Domain.Enums;
    using System.Web.Mvc;
    using System.Web.Routing;
    using System.Linq;

    public class RequiresAuthenticationAttribute : ActionFilterAttribute
    {
        public UserProfile[] DeniedUserProfiles { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var session = filterContext.HttpContext.Session;

            if (session != null && session["UserLogged"] != null)
            {
                if (DeniedUserProfiles != null)
                {
                    User userLogged = session["UserLogged"] as User;

                    if (!DeniedUserProfiles.Contains(userLogged.Profile))
                    {
                        return;
                    }
                }
                else
                { 
                    return;
                }
            }

            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                filterContext.Result = new JsonResult
                {
                    Data = new { success = false, message = "error:unauthenticated" },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            else
            {
                var route = new RouteValueDictionary(new { controller = "Account", action = "Login" });
                filterContext.Result = new RedirectToRouteResult(route);
            }
        }
    }
}