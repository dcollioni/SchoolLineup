namespace SchoolLineup.Web.Mvc
{
    using Castle.Windsor;
    using Castle.Windsor.Installer;
    using CommonServiceLocator.WindsorAdapter;
    using Microsoft.Practices.ServiceLocation;
    using SchoolLineup.Web.Mvc.App_Start;
    using SchoolLineup.Web.Mvc.CastleWindsor;
    using SchoolLineup.Web.Mvc.ModelBinding;
    using SharpArch.Domain.Events;
    using SharpArch.Web.Mvc.Castle;
    using System.Web.Http;
    using System.Web.Http.Dispatcher;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            this.InitializeServiceLocator();

            ModelBinders.Binders.DefaultBinder = new U413ModelBinder();

            AreaRegistration.RegisterAllAreas();
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        /// <summary>
        /// Instantiate the container and add all Controllers that derive from
        /// WindsorController to the container.  Also associate the Controller
        /// with the WindsorContainer ControllerFactory.
        /// </summary>
        protected virtual void InitializeServiceLocator()
        {
            IWindsorContainer container = new WindsorContainer();
            container.Install(FromAssembly.This());

            ControllerBuilder.Current.SetControllerFactory(new WindsorControllerFactory(container));

            GlobalConfiguration.Configuration.Services.Replace(
                typeof(IHttpControllerActivator),
                new WindsorCompositionRoot(container));

            var windsorServiceLocator = new WindsorServiceLocator(container);

            DomainEvents.ServiceLocator = windsorServiceLocator;

            ServiceLocator.SetLocatorProvider(() => windsorServiceLocator);
        }
    }
}