namespace SchoolLineup.Web.Mvc.CastleWindsor
{
    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.SubSystems.Configuration;
    using Castle.Windsor;

    public class QueryInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Classes.FromAssemblyNamed("SchoolLineup.Web.Mvc")
                    .InNamespace("SchoolLineup.Web.Mvc.Controllers.Queries", true)
                    .LifestylePerWebRequest()
                    .WithService.DefaultInterfaces());
        }
    }
}