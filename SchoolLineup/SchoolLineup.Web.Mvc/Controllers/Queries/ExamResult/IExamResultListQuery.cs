namespace SchoolLineup.Web.Mvc.Controllers.Queries.ExamResult
{
    using SchoolLineup.Web.Mvc.Controllers.ViewModels;
    using SchoolLineup.Domain.Entities;
    using System.Collections.Generic;

    public interface IExamResultListQuery
    {
        ExamResult Get(int id);
        IEnumerable<ExamResult> Get(int[] ids);
        IEnumerable<ExamResultViewModel> GetAllByExam(int examId);
        IEnumerable<ExamResultViewModel> GetAll(int studentId, int partialGradeId);
        IEnumerable<ExamResultViewModel> GetTotals(int studentId, int courseId);
    }
}