namespace SchoolLineup.Web.Mvc.Controllers
{
    using SchoolLineup.Domain.Contracts.Tasks;
    using SchoolLineup.Domain.Entities;
    using SchoolLineup.Tasks.Commands.ExamResult;
    using SchoolLineup.Web.Mvc.ActionFilters;
    using SchoolLineup.Web.Mvc.Controllers.Queries.College;
    using SchoolLineup.Web.Mvc.Controllers.Queries.Course;
    using SchoolLineup.Web.Mvc.Controllers.Queries.Exam;
    using SchoolLineup.Web.Mvc.Controllers.Queries.ExamResult;
    using SchoolLineup.Web.Mvc.Controllers.Queries.PartialGrade;
    using SchoolLineup.Web.Mvc.Controllers.ViewModels;
    using SharpArch.Domain.Commands;
    using SharpArch.RavenDb.Web.Mvc;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Net;
    using System.Net.Mail;
    using System.Threading.Tasks;
    using System.Web.Mvc;

    [RequiresAuthentication]
    public class ExamResultController : BaseController
    {
        private readonly ICommandProcessor commandProcessor;
        private readonly IExamListQuery examListQuery;
        private readonly IExamResultListQuery examResultListQuery;
        private readonly IExamTasks examTasks;
        private readonly IPartialGradeListQuery partialGradeListQuery;
        private readonly ICourseListQuery courseListQuery;
        private readonly ICollegeListQuery collegeListQuery;

        private IEnumerable<ExamResult> examResults;

        public ExamResultController(ICommandProcessor commandProcessor,
                                    IExamListQuery examListQuery,
                                    IExamResultListQuery examResultListQuery,
                                    IExamTasks examTasks,
                                    IPartialGradeListQuery partialGradeListQuery,
                                    ICourseListQuery courseListQuery,
                                    ICollegeListQuery collegeListQuery)
        {
            this.commandProcessor = commandProcessor;
            this.examListQuery = examListQuery;
            this.examResultListQuery = examResultListQuery;
            this.examTasks = examTasks;
            this.partialGradeListQuery = partialGradeListQuery;
            this.courseListQuery = courseListQuery;
            this.collegeListQuery = collegeListQuery;
        }

        public ActionResult Index(int examId)
        {
            var exam = examListQuery.Get(examId);

            if (exam != null)
            {
                ViewBag.ExamId = exam.Id;
                ViewBag.ExamName = exam.Name;
                ViewBag.ExamValue = exam.Value.ToString("F2", new CultureInfo("en-us"));
                ViewBag.ExamDate = exam.Date.ToString("d", new CultureInfo("pt-br"));

                var partialGrade = partialGradeListQuery.Get(exam.PartialGradeId);
                ViewBag.PartialGradeName = partialGrade.Name;

                var course = courseListQuery.Get(partialGrade.CourseId);
                ViewBag.CourseId = course.Id;
                ViewBag.CourseName = course.Name;

                var college = collegeListQuery.Get(course.CollegeId);
                ViewBag.CollegeId = college.Id;
                ViewBag.CollegeName = college.Name;
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

        public async Task SendResultsByEmail()
        {
            using (var smtpClient = new SmtpClient())
            {
                var message = new MailMessage("resultados@graduare.com", "dcollioni@gmail.com", "[Graduare] Resultados", "Teste de e-mail...");

                await smtpClient.SendMailAsync(message);
            }
        }
    }
}