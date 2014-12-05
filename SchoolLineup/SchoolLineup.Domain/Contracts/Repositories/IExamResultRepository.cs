namespace SchoolLineup.Domain.Contracts.Repositories
{
    using SchoolLineup.Domain.Entities;
    using SharpArch.Domain.PersistenceSupport;

    public interface IExamResultRepository : IRepository<ExamResult>
    {
        void Evict(ExamResult entity);
        void Delete(int entityId);
        int CountByStudent(int studentId);
    }
}