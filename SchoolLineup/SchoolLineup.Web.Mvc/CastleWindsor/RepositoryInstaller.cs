﻿namespace SchoolLineup.Web.Mvc.CastleWindsor
{
    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.SubSystems.Configuration;
    using Castle.Windsor;
    using SharpArch.Domain.PersistenceSupport;
    using SharpArch.RavenDb;
    using SharpArch.RavenDb.Contracts.Repositories;

    public class RepositoryInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            AddGenericRepositoriesTo(container);
            AddCustomRepositoriesTo(container);
        }

        private static void AddCustomRepositoriesTo(IWindsorContainer container)
        {
            container.Register(
                Types.FromAssemblyNamed("SchoolLineup.Infrastructure")
                    .BasedOn(typeof(IRepositoryWithTypedId<,>))
                    .WithService.DefaultInterfaces().LifestyleTransient());
        }

        private static void AddGenericRepositoriesTo(IWindsorContainer container)
        {
            container.Register(
                Component.For(typeof(IRavenDbRepository<>))
                    .ImplementedBy(typeof(RavenDbRepository<>))
                    .Named("ravenDbRepositoryType")
                    .Forward(typeof(IRepository<>), typeof(ILinqRepository<>))
                    .LifestyleTransient());

            container.Register(
                Component.For(typeof(IRavenDbRepositoryWithTypedId<,>))
                    .ImplementedBy(typeof(RavenDbRepositoryWithTypedId<,>))
                    .Named("ravenDbRepositoryWithTypedId")
                    .Forward(typeof(IRepositoryWithTypedId<,>), typeof(ILinqRepositoryWithTypedId<,>))
                    .LifestyleTransient());
        }
    }
}