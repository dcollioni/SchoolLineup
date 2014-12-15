namespace SchoolLineup.Web.Mvc.Controllers.Queries.ExamResult
{
    using Raven.Client;
    using Raven.Client.Linq;
    using SchoolLineup.Domain.Entities;
    using SchoolLineup.Web.Mvc.Controllers.ViewModels;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    public class ExamResultListQuery : IExamResultListQuery
    {
        private readonly IDocumentSession session;

        public ExamResultListQuery(IDocumentSession session)
        {
            this.session = session;
        }

        public ExamResult Get(int id)
        {
            return session.Load<ExamResult>(id);
        }

        public IEnumerable<ExamResult> Get(int[] ids)
        {
            return session.Query<ExamResult>()
                          .Where(e => e.Id.In(ids))
                          .ToList();
        }

        public IEnumerable<ExamResultViewModel> GetAll(int studentId, int partialGradeId)
        {
            var viewModels = new List<ExamResultViewModel>();

            var examsIds = session.Query<Exam>()
                                        .Where(e => e.PartialGradeId == partialGradeId)
                                        .Select(e => e.Id)
                                        .ToArray<int>();

            var examResults = session.Query<ExamResult>()
                                     .Customize(e => e.Include<ExamResult, Exam>(x => x.ExamId))
                                     .Where(e => e.ExamId.In(examsIds) && e.StudentId == studentId)
                                     .ToList();

            var brCulture = new CultureInfo("pt-BR");

            foreach (var examResult in examResults)
            {
                var exam = session.Load<Exam>(examResult.ExamId);

                viewModels.Add(new ExamResultViewModel()
                {
                    Description = examResult.Description,
                    ExamDate = exam.Date,
                    ExamDateStr = exam.Date.ToString("d", brCulture),
                    ExamDescription = exam.Description,
                    ExamId = exam.Id,
                    ExamName = exam.Name,
                    ExamValue = exam.Value,
                    Id = examResult.Id,
                    StudentId = examResult.StudentId,
                    Value = examResult.Value
                });
            }

            return viewModels.OrderBy(v => v.ExamDate);
        }

        public IEnumerable<ExamResultViewModel> GetTotals(int studentId, int courseId)
        {
            var viewModels = new List<ExamResultViewModel>();

            var partialGrades = session.Query<PartialGrade>()
                                          .Where(p => p.CourseId == courseId)
                                          .OrderBy(p => p.Order)
                                          .ToList();

            foreach (var partialGrade in partialGrades)
            {
                var viewModel = new ExamResultViewModel()
                {
                    ExamName = partialGrade.Name
                };

                var exams = session.Query<Exam>()
                                   .Where(e => e.PartialGradeId == partialGrade.Id)
                                   .ToList();

                var examsResults = session.Query<ExamResult>()
                                          .Where(e => e.ExamId.In(exams.Select(x => x.Id)) && e.StudentId == studentId)
                                          .ToList();

                viewModel.ExamValue = exams.Sum(e => e.Value);
                viewModel.Value = examsResults.Sum(e => e.Value);
                viewModel.ExamDateStr = string.Empty;

                viewModels.Add(viewModel);
            }

            return viewModels;
        }

        public IEnumerable<ExamResultViewModel> GetAllByExam(int examId)
        {
            var result = new List<ExamResultViewModel>();

            var exam = session.Load<Exam>(examId);

            if (exam != null)
            {
                var partialGrade = session.Load<PartialGrade>(exam.PartialGradeId);

                if (partialGrade != null)
                {
                    var course = session.Load<Course>(partialGrade.CourseId);

                    if (course != null)
                    {
                        var students = session.Query<Student>()
                                              .Where(s => s.Id.In(course.StudentsIds))
                                              .OrderBy(s => s.Name)
                                              .ToList();

                        var examResults = session.Query<ExamResult>()
                                                 .Where(e => e.ExamId == examId)
                                                 .ToList();

                        foreach (var student in students)
                        {
                            var vm = new ExamResultViewModel()
                            {
                                ExamId = examId,
                                StudentId = student.Id,
                                StudentName = student.Name
                            };

                            var examResult = examResults.SingleOrDefault(e => e.StudentId == vm.StudentId);

                            if (examResult != null)
                            {
                                vm.Description = examResult.Description;
                                vm.Id = examResult.Id;
                                vm.Value = examResult.Value;
                            }

                            result.Add(vm);
                        }
                    }
                }
            }

            return result;
        }
    }
}