namespace SchoolLineup.Web.Mvc.Controllers
{
    using SchoolLineup.Domain.Contracts.Tasks;
    using SchoolLineup.Domain.Entities;
    using SchoolLineup.Tasks.Commands.PartialGrade;
    using SchoolLineup.Web.Mvc.Controllers.Queries.PartialGrade;
    using SchoolLineup.Web.Mvc.Controllers.ViewModels;
    using SharpArch.Domain.Commands;
    using SharpArch.RavenDb.Web.Mvc;
    using System.Web.Mvc;

    public class PartialGradeController : BaseController
    {
        private readonly ICommandProcessor commandProcessor;
        private readonly IPartialGradeListQuery partialGradeListQuery;
        private readonly IPartialGradeTasks partialGradeTasks;

        public PartialGradeController(ICommandProcessor commandProcessor,
                                      IPartialGradeListQuery partialGradeListQuery,
                                      IPartialGradeTasks partialGradeTasks)
        {
            this.commandProcessor = commandProcessor;
            this.partialGradeListQuery = partialGradeListQuery;
            this.partialGradeTasks = partialGradeTasks;
        }

        public JsonResult GetAll(int courseId)
        {
            var data = partialGradeListQuery.GetAllByCourse(courseId);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [Transaction]
        public JsonResult Save(PartialGradeViewModel viewModel)
        {
            var entity = GetEntity(viewModel);

            var command = new SavePartialGradeCommand(entity, partialGradeTasks);

            this.commandProcessor.Process(command);

            if (!command.Success)
            {
                return Json(new { Success = false, Messages = command.ValidationResults() });
            }

            viewModel = GetViewModel(command.Entity);

            return Json(new { Success = true, Data = viewModel });
        }

        [Transaction]
        public JsonResult Delete(int id)
        {
            var command = new DeletePartialGradeCommand(id, partialGradeTasks);

            this.commandProcessor.Process(command);

            if (!command.Success)
            {
                return Json(new { Success = false, Messages = command.ValidationResults() });
            }

            return Json(new { Success = true });
        }

        private PartialGrade GetEntity(PartialGradeViewModel viewModel)
        {
            var entity = new PartialGrade();

            if (viewModel.Id > 0)
            {
                entity = partialGradeListQuery.Get(viewModel.Id);
            }

            entity.Name = GetTrimOrNull(viewModel.Name);
            entity.Order = viewModel.Order;
            entity.CourseId = viewModel.CourseId;

            return entity;
        }

        private PartialGradeViewModel GetViewModel(PartialGrade entity)
        {
            var viewModel = new PartialGradeViewModel();

            viewModel.Id = entity.Id;
            viewModel.Name = entity.Name;
            viewModel.Order = entity.Order;
            viewModel.CourseId = entity.CourseId;

            return viewModel;
        }
    }
}