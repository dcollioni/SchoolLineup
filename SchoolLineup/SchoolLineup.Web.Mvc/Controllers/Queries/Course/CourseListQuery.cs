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

            var courses = session.Query<Course>()
                                 .Customize(x => x.Include<Course, College>(e => e.CollegeId))
                                 .Customize(x => x.Include<Course, Teacher>(e => e.TeacherId))
                                 .Where(c => c.StudentsIds.Any(id => id == studentId))
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

        public IList<CourseViewModel> GetAllByCollege(int collegeId)
        {
            var brCulture = new CultureInfo("pt-BR");

            var courses =  session.Query<Course>()
                                  .Where(c => c.CollegeId == collegeId)
                                  .Select(e => new CourseViewModel()
                                  {
                                        Id = e.Id,
                                        Name = e.Name,
                                        CollegeId = e.CollegeId,
                                        FinishDate = e.FinishDate,
                                        IsClosed = e.IsClosed,
                                        StartDate = e.StartDate,
                                        TeacherId = e.TeacherId
                                  })
                                  .OrderBy(e => e.Name)
                                  .ToList<CourseViewModel>();

            courses.ForEach(c => c.StartDateStr = c.StartDate.ToString("d", brCulture));
            courses.ForEach(c => c.FinishDateStr = c.FinishDate.ToString("d", brCulture));

            return courses.ToList();
        }

        public Course Get(int id)
        {
            return session.Load<Course>(id);
        }
    }
}