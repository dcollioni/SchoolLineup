namespace SchoolLineup.Domain.Contracts.Repositories
{
    using SchoolLineup.Domain.Entities;
    using SharpArch.Domain.PersistenceSupport;
    using System.Collections.Generic;

    public interface IStudentRepository : IRepository<Student>
    {
        void Evict(Student entity);
        void Delete(int entityId);
        int CountByEmail(Student entity);
        int CountByEmailList(IEnumerable<string> emails);
    }
}