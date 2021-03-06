﻿namespace SchoolLineup.Domain.Contracts.Repositories
{
    using SchoolLineup.Domain.Entities;
    using SharpArch.Domain.PersistenceSupport;

    public interface ISchoolRepository : IRepository<School>
    {
        void Evict(School entity);
        void Delete(int entityId);
        int CountByName(School entity);
    }
}