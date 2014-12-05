﻿namespace SchoolLineup.Infrastructure.Repositories
{
    using Raven.Client;
    using SchoolLineup.Domain.Contracts.Repositories;
    using SchoolLineup.Domain.Entities;
    using System;
    using System.Linq;

    public class StudentRepository : BaseRepository<Student>, IStudentRepository
    {
        public StudentRepository(IDocumentSession session)
            : base(session)
        {
        }

        public int CountByEmail(Student entity)
        {
            return Session.Query<Student>()
                          .Count(e => e.Email.Equals(entity.Email, StringComparison.OrdinalIgnoreCase)
                                   && e.Id != entity.Id);
        }
    }
}