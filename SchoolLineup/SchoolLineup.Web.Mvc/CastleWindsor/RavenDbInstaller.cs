namespace SchoolLineup.Web.Mvc.CastleWindsor
{
    using Castle.MicroKernel;
    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.SubSystems.Configuration;
    using Castle.Windsor;
    using Raven.Client;
    using Raven.Client.Document;

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

            store.Conventions.FindIdentityProperty = prop => prop.Name == "DocumentId";

            store.Conventions.FindTypeTagName = type =>
            {
                var s = DocumentConvention.DefaultTypeTagName(type);
                return s.Substring(0, 1).ToLower() + s.Substring(1);
            };

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