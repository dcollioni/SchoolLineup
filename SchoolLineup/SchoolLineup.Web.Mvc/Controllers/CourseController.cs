namespace SchoolLineup.Web.Mvc.Controllers
{
    using SchoolLineup.Domain.Contracts.Tasks;
    using SchoolLineup.Domain.Entities;
    using SchoolLineup.Tasks.Commands.Course;
    using SchoolLineup.Web.Mvc.ActionFilters;
    using SchoolLineup.Web.Mvc.Controllers.Queries.College;
    using SchoolLineup.Web.Mvc.Controllers.Queries.Course;
    using SchoolLineup.Web.Mvc.Controllers.Queries.Student;
    using SchoolLineup.Web.Mvc.Controllers.Queries.Teacher;
    using SchoolLineup.Web.Mvc.Controllers.ViewModels;
    using SharpArch.Domain.Commands;
    using SharpArch.RavenDb.Web.Mvc;
    using System.Globalization;
    using System.Web.Mvc;

    [RequiresAuthentication]
    public class CourseController : BaseController
    {
        private readonly ICommandProcessor commandProcessor;
        private readonly ICollegeListQuery collegeListQuery;
        private readonly ICourseListQuery courseListQuery;
        private readonly ITeacherListQuery teacherListQuery;
        private readonly ICourseTasks courseTasks;
        private readonly IStudentListQuery studentListQuery;

        public CourseController(ICommandProcessor commandProcessor,
                                ICollegeListQuery collegeListQuery,
                                ICourseListQuery courseListQuery,
                                ITeacherListQuery teacherListQuery,
                                ICourseTasks courseTasks,
                                IStudentListQuery studentListQuery)
        {
            this.commandProcessor = commandProcessor;
            this.collegeListQuery = collegeListQuery;
            this.courseListQuery = courseListQuery;
            this.teacherListQuery = teacherListQuery;
            this.courseTasks = courseTasks;
            this.studentListQuery = studentListQuery;
        }

        public ActionResult Index(int? collegeId)
        {
            if (collegeId.HasValue)
            {
                var college = collegeListQuery.Get(collegeId.Value);

                if (college != null)
                {
                    ViewBag.CollegeName = college.Name;
                    ViewBag.CollegeId = college.Id;

                    return View();
                }
            }

            return RedirectToAction("Index", "College");
        }

        public ActionResult Dashboard(int? id)
        {
            if (id.HasValue)
            {
                var course = courseListQuery.Get(id.Value);

                if (course != null)
                {
                    ViewBag.CourseName = course.Name;
                    ViewBag.CourseId = course.Id;
                    ViewBag.StartDate = course.StartDate.ToString("d", new CultureInfo("pt-br"));
                    ViewBag.FinishDate = course.FinishDate.ToString("d", new CultureInfo("pt-br"));
                    ViewBag.IsClosed = course.IsClosed ? "Sim" : "Não";

                    var teacher = teacherListQuery.Get(course.TeacherId);
                    ViewBag.TeacherName = teacher.Name;

                    var college = collegeListQuery.Get(course.CollegeId);
                    ViewBag.CollegeId = college.Id;
                    ViewBag.CollegeName = college.Name;

                    return View();
                }
            }

            return RedirectToAction("Index", "College");
        }

        public JsonResult GetAll(int collegeId)
        {
            var data = courseListQuery.GetAllByCollege(collegeId);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTeachers()
        {
            var data = teacherListQuery.GetAll();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [Transaction]
        public JsonResult Save(CourseViewModel viewModel)
        {
            var entity = GetEntity(viewModel);

            var command = new SaveCourseCommand(entity, courseTasks);

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
            var command = new DeleteCourseCommand(id, courseTasks);

            this.commandProcessor.Process(command);

            if (!command.Success)
            {
                return Json(new { Success = false, Messages = command.ValidationResults() });
            }

            return Json(new { Success = true });
        }

        public JsonResult GetAllStudents(int courseId)
        {
            var data = studentListQuery.GetAllByCourse(courseId);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [Transaction]
        public JsonResult AddStudent(int courseId, int studentId)
        {
            var course = courseListQuery.Get(courseId);

            if (course != null)
            {
                if (studentId > 0 && !course.StudentsIds.Contains(studentId))
                {
                    course.StudentsIds.Add(studentId);

                    var command = new SaveCourseCommand(course, courseTasks);
                    this.commandProcessor.Process(command);

                    if (!command.Success)
                    {
                        return Json(new { Success = false, Messages = command.ValidationResults() });
                    }
                }
            }

            return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
        }

        [Transaction]
        public JsonResult RemoveStudent(int courseId, int studentId)
        {
            var course = courseListQuery.Get(courseId);

            if (course != null)
            {
                if (studentId > 0 && course.StudentsIds.Contains(studentId))
                {
                    course.StudentsIds.Remove(studentId);

                    var command = new SaveCourseCommand(course, courseTasks);
                    this.commandProcessor.Process(command);

                    if (!command.Success)
                    {
                        return Json(new { Success = false, Messages = command.ValidationResults() });
                    }
                }
            }

            return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
        }

        private Course GetEntity(CourseViewModel viewModel)
        {
            var entity = new Course();

            if (viewModel.Id > 0)
            {
                entity = courseListQuery.Get(viewModel.Id);
            }

            entity.Name = GetTrimOrNull(viewModel.Name);
            entity.CollegeId = viewModel.CollegeId;
            entity.TeacherId = viewModel.TeacherId;
            entity.StartDate = viewModel.StartDate;
            entity.FinishDate = viewModel.FinishDate;
            entity.IsClosed = viewModel.IsClosed;

            return entity;
        }

        private CourseViewModel GetViewModel(Course entity)
        {
            var viewModel = new CourseViewModel();

            viewModel.Id = entity.Id;
            viewModel.Name = entity.Name;
            viewModel.CollegeId = entity.CollegeId;
            viewModel.TeacherId = entity.TeacherId;
            viewModel.FinishDate = entity.FinishDate;
            viewModel.StartDate = entity.StartDate;
            viewModel.IsClosed = entity.IsClosed;

            return viewModel;
        }
    }
}