﻿namespace SchoolLineup.Web.Mvc.Controllers.Queries.PartialGrade
{
    using Raven.Client;
    using Raven.Client.Linq;
    using SchoolLineup.Domain.Entities;
    using SchoolLineup.Web.Mvc.Controllers.ViewModels;
    using System.Collections.Generic;
    using System.Linq;

    public class PartialGradeListQuery : IPartialGradeListQuery
    {
        private readonly IDocumentSession session;

        public PartialGradeListQuery(IDocumentSession session)
        {
            this.session = session;
        }

        public IEnumerable<PartialGrade> GetAll(int courseId)
        {
            return session.Query<PartialGrade>()
                          .Where(p => p.CourseId == courseId)
                          .OrderBy(p => p.Order)
                          .ToList();
        }

        public IEnumerable<PartialGradeViewModel> GetAllByCourse(int courseId)
        {
            return session.Query<PartialGrade>()
                          .Where(p => p.CourseId == courseId)
                          .OrderBy(p => p.Order)
                          .Select(p => new PartialGradeViewModel()
                          {
                              CourseId = p.CourseId,
                              Id = p.Id,
                              Name = p.Name,
                              Order = p.Order
                          })
                          .ToList();
        }

        public PartialGrade Get(int id)
        {
            return session.Load<PartialGrade>(id);
        }
    }
}