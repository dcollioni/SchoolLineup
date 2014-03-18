using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace SchoolLineup.Web.Mvc.CastleWindsor
{
    public class TasksInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Types.FromAssemblyNamed("SchoolLineup.Tasks")
                    .Pick().Unless(t => t.Namespace.EndsWith("Handlers"))
                    .WithService.DefaultInterfaces()
                    .Configure(c => c.LifestyleTransient())
                );
        }
    }
}