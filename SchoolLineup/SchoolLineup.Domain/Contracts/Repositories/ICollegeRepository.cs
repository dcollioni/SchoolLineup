namespace SchoolLineup.Domain.Contracts.Repositories
{
    using SchoolLineup.Domain.Entities;
    using SharpArch.Domain.PersistenceSupport;

    public interface ICollegeRepository : IRepository<College>
    {
        void Evict(College entity);
        void Delete(int entityId);
    }
}