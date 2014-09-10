namespace SchoolLineup.Web.Mvc.Controllers
{
    using SchoolLineup.Domain.Contracts.Repositories;
    using SchoolLineup.Domain.Contracts.Tasks;
    using SchoolLineup.Domain.Entities;
    using SchoolLineup.Tasks.Commands.School;
    using SchoolLineup.Web.Mvc.Controllers.Queries.School;
    using SchoolLineup.Web.Mvc.Controllers.ViewModels;
    using SharpArch.Domain.Commands;
    using SharpArch.RavenDb.Web.Mvc;
    using System.Globalization;
    using System.Web.Mvc;

    public class SchoolController : BaseController
    {
        private readonly ISchoolListQuery schoolListQuery;
        private readonly ISchoolTasks schoolTasks;
        private readonly ICommandProcessor commandProcessor;
        private readonly ISchoolRepository schoolRepository;

        public SchoolController(ISchoolListQuery schoolListQuery,
                                ISchoolTasks schoolTasks,
                                ICommandProcessor commandProcessor,
                                ISchoolRepository schoolRepository)
        {
            this.schoolListQuery = schoolListQuery;
            this.schoolTasks = schoolTasks;
            this.commandProcessor = commandProcessor;
            this.schoolRepository = schoolRepository;
        }

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetAll()
        {
            var schools = schoolListQuery.GetAll();

            return Json(schools, JsonRequestBehavior.AllowGet);
        }

        [Transaction]
        public JsonResult Save(SchoolViewModel viewModel)
        {
            var entity = GetEntity(viewModel);

            var command = new SaveSchoolCommand(entity, schoolTasks);

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
            var command = new DeleteSchoolCommand(id, schoolTasks);

            this.commandProcessor.Process(command);

            if (!command.Success)
            {
                return Json(new { Success = false, Messages = command.ValidationResults() });
            }

            return Json(new { Success = true });
        }

        public JsonResult IsNameUnique(SchoolViewModel viewModel)
        {
            var entity = GetEntity(viewModel);
            var isNameUnique = schoolTasks.IsNameUnique(entity);

            return Json(isNameUnique, JsonRequestBehavior.AllowGet);
        }

        private School GetEntity(SchoolViewModel viewModel)
        {
            var entity = new School();

            if (viewModel.Id > 0)
            {
                entity = schoolListQuery.Get(viewModel.Id);
            }

            entity.Name = GetTrimOrNull(viewModel.Name);
            entity.Email = GetTrimOrNull(viewModel.Email);
            entity.ManagerName = GetTrimOrNull(viewModel.ManagerName);
            entity.Phone = GetPhoneNumber(viewModel.Phone);

            return entity;
        }

        private SchoolViewModel GetViewModel(School entity)
        {
            var viewModel = new SchoolViewModel();

            viewModel.Id = entity.Id;
            viewModel.Name = entity.Name;
            viewModel.Email = entity.Email;
            viewModel.ManagerName = entity.ManagerName;
            viewModel.Phone = entity.Phone;

            return viewModel;
        }

        [Transaction]
        public void ImportData()
        {
            using (var reader = System.IO.File.OpenText(Server.MapPath("~/App_Data/escolas.txt")))
            {
                while (!reader.EndOfStream)
                {
                    var schoolData = reader.ReadLine().Split(':');

                    schoolRepository.SaveOrUpdate
                    (
                        new School()
                        {
                            Address = schoolData[3].Trim(),
                            Code = schoolData[0].Trim(),
                            Lat = double.Parse(schoolData[2].Split(',')[0].Trim(), new CultureInfo("en-US")),
                            Lng = double.Parse(schoolData[2].Split(',')[1].Trim(), new CultureInfo("en-US")),
                            Name = schoolData[1].Trim(),
                            Phone = schoolData[4].Trim(),
                            SiteCode = schoolData[5].Trim()
                        }
                    );
                }
            }
        }
    }
}