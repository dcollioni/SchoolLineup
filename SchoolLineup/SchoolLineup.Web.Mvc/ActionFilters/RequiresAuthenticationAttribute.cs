namespace SchoolLineup.Web.Mvc.ActionFilters
{
    using System.Web.Mvc;
    using System.Web.Routing;

    public class RequiresAuthenticationAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var session = filterContext.HttpContext.Session;

            if (session != null && session["UserLogged"] != null) return;

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
                var route = new RouteValueDictionary(new { controller = "User", action = "Login" });
                filterContext.Result = new RedirectToRouteResult(route);
            }
        }
    }
}