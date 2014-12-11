namespace SchoolLineup.Domain.Contracts.Repositories
{
    using SchoolLineup.Domain.Entities;
    using SharpArch.Domain.PersistenceSupport;

    public interface IExamRepository : IRepository<Exam>
    {
        void Evict(Exam entity);
        void Delete(int entityId);
        int CountByPartialGrade(int partialGradeId);
    }
}