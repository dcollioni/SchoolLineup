namespace SchoolLineup.Web.Mvc.Controllers.Queries.Exam
{
    using SchoolLineup.Domain.Entities;
    using SchoolLineup.Web.Mvc.Controllers.ViewModels;
    using System.Collections.Generic;

    public interface IExamListQuery
    {
        Exam Get(int id);
        IEnumerable<ExamViewModel> GetAll(int partialGradeId);
    }
}