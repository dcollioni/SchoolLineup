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
    }
}