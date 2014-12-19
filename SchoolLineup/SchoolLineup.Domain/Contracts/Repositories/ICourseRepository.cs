namespace SchoolLineup.Domain.Contracts.Repositories
{
    using SchoolLineup.Domain.Entities;
    using SharpArch.Domain.PersistenceSupport;

    public interface ICourseRepository : IRepository<Course>
    {
        void Evict(Course entity);
        void Delete(int entityId);
        int CountByCollege(int collegeId);
        int CountByTeacher(int teacherId);
    }
}