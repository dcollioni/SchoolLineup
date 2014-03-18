using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using SharpArch.Domain.Commands;

namespace SchoolLineup.Web.Mvc.CastleWindsor
{
    public class SharpArchInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For(typeof(ICommandProcessor))
                    .ImplementedBy(typeof(CommandProcessor))
                    .Named("commandProcessor"));
        }
    }
}