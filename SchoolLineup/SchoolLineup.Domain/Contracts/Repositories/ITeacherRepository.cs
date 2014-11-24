namespace SchoolLineup.Domain.Contracts.Repositories
{
    using SchoolLineup.Domain.Entities;
    using SharpArch.Domain.PersistenceSupport;

    public interface ITeacherRepository : IRepository<Teacher>
    {
        void Evict(Teacher entity);
        void Delete(int entityId);
    }
}