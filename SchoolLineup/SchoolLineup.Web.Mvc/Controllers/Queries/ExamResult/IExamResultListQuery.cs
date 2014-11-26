namespace SchoolLineup.Web.Mvc.Controllers.Queries.ExamResult
{
    using SchoolLineup.Web.Mvc.Controllers.ViewModels;
    using System.Collections.Generic;

    public interface IExamResultListQuery
    {
        IEnumerable<ExamResultViewModel> GetAll(int studentId, int partialGradeId);
        IEnumerable<ExamResultViewModel> GetTotals(int studentId, int courseId);
    }
}