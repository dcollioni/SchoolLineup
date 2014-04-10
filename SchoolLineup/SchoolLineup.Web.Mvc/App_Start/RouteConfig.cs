namespace SchoolLineup.Web.Mvc
{
    using System.Web.Mvc;
    using System.Web.Routing;

    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            RegisterCourseRoutes(routes);

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "SchoolLineup.Web.Mvc.Controllers" }
            );
        }

        private static void RegisterCourseRoutes(RouteCollection routes)
        {
            routes.MapRoute(
                name: "SaveCourse",
                url: "Course/Save",
                defaults: new { controller = "Course", action = "Save" },
                namespaces: new[] { "SchoolLineup.Web.Mvc.Controllers" }
            );

            routes.MapRoute(
                name: "CoursesBySchool",
                url: "Course/{id}",
                defaults: new { controller = "Course", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "SchoolLineup.Web.Mvc.Controllers" }
            );
        }
    }
}