namespace SchoolLineup.Web.Mvc.CastleWindsor
{
    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.SubSystems.Configuration;
    using Castle.Windsor;
    using SharpArch.Domain.Commands;
    using SharpArch.Domain.Events;

    public class HandlerInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Types.FromAssemblyNamed("SchoolLineup.Tasks")
                    .BasedOn(typeof(ICommandHandler<>))
                    .WithService.FirstInterface().LifestyleTransient());

            container.Register(
                Types.FromAssemblyNamed("SchoolLineup.Tasks")
                    .BasedOn(typeof(ICommandHandler<,>))
                    .WithService.FirstInterface().LifestyleTransient());

            container.Register(
                Types.FromAssemblyNamed("SchoolLineup.Tasks")
                    .BasedOn(typeof(IHandles<>))
                    .WithService.FirstInterface().LifestyleTransient());
        }
    }
}