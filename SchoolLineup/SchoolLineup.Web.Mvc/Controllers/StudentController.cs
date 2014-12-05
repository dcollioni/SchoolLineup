﻿namespace SchoolLineup.Web.Mvc.Controllers
{
    using SchoolLineup.Domain.Contracts.Tasks;
    using SchoolLineup.Domain.Entities;
    using SchoolLineup.Tasks.Commands.Student;
    using SchoolLineup.Web.Mvc.Controllers.Queries.Student;
    using SchoolLineup.Web.Mvc.Controllers.ViewModels;
    using SharpArch.Domain.Commands;
    using SharpArch.RavenDb.Web.Mvc;
    using System.Web.Mvc;

    public class StudentController : BaseController
    {
        private readonly ICommandProcessor commandProcessor;
        private readonly IStudentListQuery studentListQuery;
        private readonly IStudentTasks studentTasks;

        public StudentController(ICommandProcessor commandProcessor,
                                 IStudentListQuery studentListQuery,
                                 IStudentTasks studentTasks)
        {
            this.commandProcessor = commandProcessor;
            this.studentListQuery = studentListQuery;
            this.studentTasks = studentTasks;
        }

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetAll()
        {
            var data = studentListQuery.GetAll();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [Transaction]
        public JsonResult Save(StudentViewModel viewModel)
        {
            var entity = GetEntity(viewModel);

            var command = new SaveStudentCommand(entity, studentTasks);

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
            var command = new DeleteStudentCommand(id, studentTasks);

            this.commandProcessor.Process(command);

            if (!command.Success)
            {
                return Json(new { Success = false, Messages = command.ValidationResults() });
            }

            return Json(new { Success = true });
        }

        public JsonResult IsEmailUnique(StudentViewModel viewModel)
        {
            var entity = GetEntity(viewModel);
            var isUnique = studentTasks.IsEmailUnique(entity);

            return Json(isUnique, JsonRequestBehavior.AllowGet);
        }

        private Student GetEntity(StudentViewModel viewModel)
        {
            var entity = new Student();

            if (viewModel.Id > 0)
            {
                entity = studentListQuery.Get(viewModel.Id);
            }

            entity.Name = GetTrimOrNull(viewModel.Name);
            entity.Email = GetTrimOrNull(viewModel.Email);
            entity.RegistrationCode = GetTrimOrNull(viewModel.RegistrationCode);

            return entity;
        }

        private StudentViewModel GetViewModel(Student entity)
        {
            var viewModel = new StudentViewModel();

            viewModel.Id = entity.Id;
            viewModel.Name = entity.Name;
            viewModel.Email = entity.Email;
            viewModel.RegistrationCode = entity.RegistrationCode;

            return viewModel;
        }
    }
}