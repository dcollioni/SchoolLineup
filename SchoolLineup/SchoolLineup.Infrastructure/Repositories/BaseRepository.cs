namespace SchoolLineup.Infrastructure.Repositories
{
    using Raven.Client;
    using SharpArch.RavenDb;

    public abstract class BaseRepository<T> : RavenDbRepository<T> where T : class
    {
        public BaseRepository(IDocumentSession session)
            : base(session)
        {
        }

        public void Evict(T entity)
        {
            Session.Advanced.Evict(entity);
        }
    }
}