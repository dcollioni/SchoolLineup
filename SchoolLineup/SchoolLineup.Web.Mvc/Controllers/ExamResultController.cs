namespace SchoolLineup.Web.Mvc.Controllers
{
    using SchoolLineup.Domain.Contracts.Tasks;
    using SchoolLineup.Domain.Entities;
    using SchoolLineup.Tasks.Commands.Exam;
    using SchoolLineup.Web.Mvc.Controllers.Queries.Exam;
    using SchoolLineup.Web.Mvc.Controllers.ViewModels;
    using SharpArch.Domain.Commands;
    using SharpArch.RavenDb.Web.Mvc;
    using System.Web.Mvc;

    public class ExamResultController : BaseController
    {
        //private readonly ICommandProcessor commandProcessor;
        //private readonly IExamListQuery examListQuery;
        //private readonly IExamTasks examTasks;

        public ExamResultController(/*ICommandProcessor commandProcessor,
                              IExamListQuery examListQuery,
                              IExamTasks examTasks*/)
        {
            //this.commandProcessor = commandProcessor;
            //this.examListQuery = examListQuery;
            //this.examTasks = examTasks;
        }

        public ActionResult Index()
        {
            return View();
        }

        //public JsonResult GetAll(int partialGradeId)
        //{
        //    var data = examListQuery.GetAll(partialGradeId);
        //    return Json(data, JsonRequestBehavior.AllowGet);
        //}

        //[Transaction]
        //public JsonResult Save(ExamViewModel viewModel)
        //{
        //    var entity = GetEntity(viewModel);

        //    var command = new SaveExamCommand(entity, examTasks);

        //    this.commandProcessor.Process(command);

        //    if (!command.Success)
        //    {
        //        return Json(new { Success = false, Messages = command.ValidationResults() });
        //    }

        //    viewModel = GetViewModel(command.Entity);

        //    return Json(new { Success = true, Data = viewModel });
        //}

        //[Transaction]
        //public JsonResult Delete(int id)
        //{
        //    var command = new DeleteExamCommand(id, examTasks);

        //    this.commandProcessor.Process(command);

        //    if (!command.Success)
        //    {
        //        return Json(new { Success = false, Messages = command.ValidationResults() });
        //    }

        //    return Json(new { Success = true });
        //}

        //private Exam GetEntity(ExamViewModel viewModel)
        //{
        //    var entity = new Exam();

        //    if (viewModel.Id > 0)
        //    {
        //        entity = examListQuery.Get(viewModel.Id);
        //    }

        //    entity.Name = GetTrimOrNull(viewModel.Name);
        //    entity.Date = viewModel.Date;
        //    entity.Value = viewModel.Value;
        //    entity.Description = GetTrimOrNull(viewModel.Description);
        //    entity.PartialGradeId = viewModel.PartialGradeId;

        //    return entity;
        //}

        //private ExamViewModel GetViewModel(Exam entity)
        //{
        //    var viewModel = new ExamViewModel();

        //    viewModel.Id = entity.Id;
        //    viewModel.Name = entity.Name;
        //    viewModel.Date = entity.Date;
        //    viewModel.Value = entity.Value;
        //    viewModel.Description = entity.Description;
        //    viewModel.PartialGradeId = entity.PartialGradeId;

        //    return viewModel;
        //}
    }
}