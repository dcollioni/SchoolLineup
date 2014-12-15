namespace SchoolLineup.Web.Mvc.Controllers
{
    using SchoolLineup.Domain.Contracts.Tasks;
    using SchoolLineup.Domain.Entities;
    using SchoolLineup.Tasks.Commands.ExamResult;
    using SchoolLineup.Web.Mvc.Controllers.Queries.Exam;
    using SchoolLineup.Web.Mvc.Controllers.Queries.ExamResult;
    using SchoolLineup.Web.Mvc.Controllers.ViewModels;
    using SharpArch.Domain.Commands;
    using SharpArch.RavenDb.Web.Mvc;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Web.Mvc;
    using System.Linq;

    public class ExamResultController : BaseController
    {
        private readonly ICommandProcessor commandProcessor;
        private readonly IExamListQuery examListQuery;
        private readonly IExamResultListQuery examResultListQuery;
        private readonly IExamTasks examTasks;

        private IEnumerable<ExamResult> examResults;

        public ExamResultController(ICommandProcessor commandProcessor,
                                    IExamListQuery examListQuery,
                                    IExamResultListQuery examResultListQuery,
                                    IExamTasks examTasks)
        {
            this.commandProcessor = commandProcessor;
            this.examListQuery = examListQuery;
            this.examResultListQuery = examResultListQuery;
            this.examTasks = examTasks;
        }

        public ActionResult Index(int examId)
        {
            var exam = examListQuery.Get(examId);

            if (exam != null)
            {
                ViewBag.ExamId = exam.Id;
                ViewBag.ExamName = exam.Name;
                ViewBag.ExamValue = exam.Value;
                ViewBag.ExamDate = exam.Date.ToString("d", new CultureInfo("pt-br"));
            }

            return View();
        }

        public JsonResult GetAll(int examId)
        {
            var data = examResultListQuery.GetAllByExam(examId);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [Transaction]
        public JsonResult Save(ExamResultViewModel[] viewModels)
        {
            examResults = examResultListQuery.Get(viewModels.Select(v => v.Id).Distinct().ToArray());

            var entities = new List<ExamResult>();

            foreach (var viewModel in viewModels)
            {
                entities.Add(GetEntity(viewModel));
            }

            var command = new SaveExamResultsCommand(entities, examTasks);

            this.commandProcessor.Process(command);

            if (!command.Success)
            {
                return Json(new { Success = false, Messages = command.ValidationResults() });
            }

            return Json(new { Success = true });
        }

        private ExamResult GetEntity(ExamResultViewModel viewModel)
        {
            var entity = new ExamResult();

            if (viewModel.Id > 0)
            {
                entity = examResults.Single(e => e.Id == viewModel.Id);
            }

            entity.Description = GetTrimOrNull(viewModel.Description);
            entity.ExamId = viewModel.ExamId;
            entity.StudentId = viewModel.StudentId;
            entity.Value = viewModel.Value;

            return entity;
        }
    }
}