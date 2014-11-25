namespace SchoolLineup.Web.Mvc.Controllers.Queries.Course
{
    using Raven.Client;
    using Raven.Client.Linq;
    using SchoolLineup.Domain.Entities;
    using SchoolLineup.Web.Mvc.Controllers.ViewModels;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    public class CourseListQuery : ICourseListQuery
    {
        private readonly IDocumentSession session;

        public CourseListQuery(IDocumentSession session)
        {
            this.session = session;
        }

        public IEnumerable<CourseViewModel> GetAll(int studentId)
        {
            var viewModels = new List<CourseViewModel>();

            var examsIds = session.Query<ExamResult>()
                                        .Where(e => e.StudentId == studentId)
                                        .Select(e => e.ExamId)
                                        .Distinct()
                                        .ToArray<int>();

            var partialGradesIds = session.Query<Exam>()
                                          .Where(e => e.Id.In(examsIds))
                                          .Select(e => e.PartialGradeId)
                                          .Distinct()
                                          .ToArray<int>();

            var coursesIds = session.Query<PartialGrade>()
                                    .Where(p => p.Id.In(partialGradesIds))
                                    .Select(p => p.CourseId)
                                    .Distinct()
                                    .ToArray<int>();

            var courses = session.Query<Course>()
                                 .Customize(x => x.Include<Course, College>(e => e.CollegeId))
                                 .Customize(x => x.Include<Course, Teacher>(e => e.TeacherId))
                                 .Where(c => c.Id.In(coursesIds))
                                 .ToList();

            var brCulture = new CultureInfo("pt-BR");

            foreach (var course in courses)
            {
                var college = session.Load<College>(course.CollegeId);
                var teacher = session.Load<Teacher>(course.TeacherId);

                viewModels.Add(new CourseViewModel()
                {
                    CollegeId = college.Id,
                    CollegeName = college.Name,
                    FinishDate = course.FinishDate,
                    Id = course.Id,
                    IsClosed = course.IsClosed,
                    Name = course.Name,
                    StartDate = course.StartDate,
                    TeacherId = teacher.Id,
                    TeacherName = teacher.Name,
                    StartDateStr = course.StartDate.ToString("d", brCulture),
                    FinishDateStr = course.FinishDate.ToString("d", brCulture)
                });
            }

            return viewModels;
        }
    }
}