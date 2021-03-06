﻿namespace SchoolLineup.Domain.Contracts.Tasks
{
    using SchoolLineup.Domain.Entities;

    public interface ICollegeTasks
    {
        bool IsNameUnique(College entity);
        bool HasChildren(int id);
    }
}