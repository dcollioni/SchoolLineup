using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Raven.Client;
using Raven.Client.Document;
using System.Configuration;

namespace SchoolLineup.Web.Mvc.CastleWindsor
{
    public class RavenDbInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<IDocumentStore>().ImplementedBy<DocumentStore>()
                    .DependsOn(new { connectionStringName = "RavenHQ" })
                    .OnCreate(DoInitialisation)
                    .LifeStyle.Singleton,
                Component.For<IDocumentSession>()
                    .UsingFactoryMethod(GetDocumentSesssion)
                    .LifeStyle.PerWebRequest
                    .OnDestroy(Destroy)
                );
        }

        static IDocumentSession GetDocumentSesssion(IKernel kernel)
        {
            var store = kernel.Resolve<IDocumentStore>();
            return store.OpenSession();
        }

        public static void Destroy(IDocumentSession session)
        {
            session.Dispose();
        }

        public static void DoInitialisation(IKernel kernel, IDocumentStore store)
        {
            store.Initialize();
        }
    }
}