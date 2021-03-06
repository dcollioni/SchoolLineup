﻿namespace SchoolLineup.Web.Mvc.CastleWindsor
{
    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.SubSystems.Configuration;
    using Castle.Windsor;
    using System.Web.Http;
    using System.Web.Http.Controllers;
    using System.Web.Mvc;

    public class ControllerInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Types
                    .FromThisAssembly()
                    .BasedOn<IController>().OrBasedOn(typeof(IHttpController))
                    .Configure(c => c.LifeStyle.Transient
                        .Named(c.Implementation.Name))
                );
        }
    }
}