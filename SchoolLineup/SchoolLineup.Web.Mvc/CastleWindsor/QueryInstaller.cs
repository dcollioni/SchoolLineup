using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace SchoolLineup.Web.Mvc.CastleWindsor
{
    public class QueryInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Types.FromAssemblyNamed("SchoolLineup.Web.Mvc")
                    .InNamespace("SchoolLineup.Web.Mvc.Controllers.Queries", true)
                    .LifestylePerWebRequest()
                    .WithService.DefaultInterfaces());
        }
    }
}