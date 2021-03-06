﻿namespace SchoolLineup.Domain.Contracts.Repositories
{
    using SchoolLineup.Domain.Entities;
    using SharpArch.Domain.PersistenceSupport;

    public interface IPartialGradeRepository : IRepository<PartialGrade>
    {
        void Evict(PartialGrade entity);
        void Delete(int entityId);
        int CountByCourse(int courseId);
        int CountByName(PartialGrade entity);
        int CountByOrder(PartialGrade entity);
    }
}