namespace SchoolLineup.Web.Mvc.Controllers
{
    using SchoolLineup.Domain.Contracts.Repositories;
    using SchoolLineup.Domain.Contracts.Tasks;
    using SchoolLineup.Domain.Entities;
    using SchoolLineup.Tasks.Commands.School;
    using SchoolLineup.Web.Mvc.ActionFilters;
    using SchoolLineup.Web.Mvc.Controllers.Queries.School;
    using SchoolLineup.Web.Mvc.Controllers.ViewModels;
    using SharpArch.Domain.Commands;
    using SharpArch.RavenDb.Web.Mvc;
    using System;
    using System.Globalization;
    using System.Security.Cryptography;
    using System.Text;
    using System.Web.Mvc;

    [RequiresAuthentication]
    public class SchoolController : BaseController
    {
        private readonly ISchoolListQuery schoolListQuery;
        private readonly ISchoolTasks schoolTasks;
        private readonly ICommandProcessor commandProcessor;
        private readonly ISchoolRepository schoolRepository;
        private readonly ICollegeRepository collegeRepository;
        private readonly ICourseRepository courseRepository;
        private readonly IExamRepository examRepository;
        private readonly IPartialGradeRepository partialGradeRepository;
        private readonly ITeacherRepository teacherRepository;
        private readonly IStudentRepository studentRepository;
        private readonly IExamResultRepository examResultRepository;

        public SchoolController(ISchoolListQuery schoolListQuery,
                                ISchoolTasks schoolTasks,
                                ICommandProcessor commandProcessor,
                                ISchoolRepository schoolRepository,
                                ICollegeRepository collegeRepository,
                                ICourseRepository courseRepository,
                                IExamRepository examRepository,
                                IPartialGradeRepository partialGradeRepository,
                                ITeacherRepository teacherRepository,
                                IStudentRepository studentRepository,
                                IExamResultRepository examResultRepository)
        {
            this.schoolListQuery = schoolListQuery;
            this.schoolTasks = schoolTasks;
            this.commandProcessor = commandProcessor;
            this.schoolRepository = schoolRepository;
            this.collegeRepository = collegeRepository;
            this.courseRepository = courseRepository;
            this.examRepository = examRepository;
            this.partialGradeRepository = partialGradeRepository;
            this.teacherRepository = teacherRepository;
            this.studentRepository = studentRepository;
            this.examResultRepository = examResultRepository;
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

        #region Import data

        //[Transaction]
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

        //[Transaction]
        public void CreateCollege()
        {
            College college = new College()
            {
                Name = "FL11"
            };

            collegeRepository.SaveOrUpdate(college);
        }

        //[Transaction]
        public void CreateTeacher()
        {
            Teacher teacher = new Teacher()
            {
                Email = "douglas.collioni@qi.edu.br",
                Name = "Douglas Collioni"
            };

            teacherRepository.SaveOrUpdate(teacher);
        }

        //[Transaction]
        public void CreateCourse()
        {
            Course course = new Course()
            {
                CollegeId = 1,
                TeacherId = 1,
                Name = "Sistema Operacional",
                StartDate = new DateTime(2014, 10, 19),
                FinishDate = new DateTime(2014, 11, 21)
            };

            courseRepository.SaveOrUpdate(course);
        }

        //[Transaction]
        public void CreatePartialGrades()
        {
            PartialGrade partialGradeN1 = new PartialGrade()
            {
                CourseId = 1,
                Name = "N1",
                Order = 1
            };

            PartialGrade partialGradeN2 = new PartialGrade()
            {
                CourseId = 1,
                Name = "N2",
                Order = 2
            };

            partialGradeRepository.SaveOrUpdate(partialGradeN1);
            partialGradeRepository.SaveOrUpdate(partialGradeN2);
        }

        //[Transaction]
        public void CreateExams()
        {
            Exam exam1 = new Exam()
            {
                PartialGradeId = 1,
                Name = "Questionário SO",
                Description = "Questionário introdutório de Sistemas Operacionais",
                Date = new DateTime(2014, 10, 22),
                Value = 2
            };

            Exam exam2 = new Exam()
            {
                PartialGradeId = 1,
                Name = "Trabalho Arquivo etc",
                Description = "Trabalho sobre os arquivo do diretório etc",
                Date = new DateTime(2014, 10, 26),
                Value = 1
            };

            Exam exam3 = new Exam()
            {
                PartialGradeId = 1,
                Name = "Trabalho Intalação Ubuntu",
                Description = "Trabalho de instalação do Ubuntu modo texto",
                Date = new DateTime(2014, 10, 30),
                Value = 2
            };

            Exam exam4 = new Exam()
            {
                PartialGradeId = 1,
                Name = "Prova 1",
                Description = "Prova da N1",
                Date = new DateTime(2014, 11, 05),
                Value = 5
            };

            Exam exam5 = new Exam()
            {
                PartialGradeId = 2,
                Name = "Trabalho Editores de Texto",
                Description = "Trabalho de pesquisa sobre diversos editores de texto do Ubuntu",
                Date = new DateTime(2014, 11, 08),
                Value = 3
            };

            Exam exam6 = new Exam()
            {
                PartialGradeId = 2,
                Name = "Questionário Permissões",
                Description = "Questionário sobre aplicação de permissões no Ubuntu",
                Date = new DateTime(2014, 11, 12),
                Value = 1
            };

            Exam exam7 = new Exam()
            {
                PartialGradeId = 2,
                Name = "Trabalho Shell Script",
                Description = "Trabalho de desenvolvimento de shell script no Ubuntu",
                Date = new DateTime(2014, 11, 16),
                Value = 2
            };

            Exam exam8 = new Exam()
            {
                PartialGradeId = 2,
                Name = "Prova 2",
                Description = "Prova da N2",
                Date = new DateTime(2014, 11, 19),
                Value = 4
            };

            examRepository.SaveOrUpdate(exam1);
            examRepository.SaveOrUpdate(exam2);
            examRepository.SaveOrUpdate(exam3);
            examRepository.SaveOrUpdate(exam4);
            examRepository.SaveOrUpdate(exam5);
            examRepository.SaveOrUpdate(exam6);
            examRepository.SaveOrUpdate(exam7);
            examRepository.SaveOrUpdate(exam8);
        }

        //[Transaction]
        public void CreateStudents()
        {
            using (var reader = System.IO.File.OpenText(Server.MapPath("~/App_Data/alunos.txt")))
            {
                MD5Cng md5 = new MD5Cng();

                while (!reader.EndOfStream)
                {
                    var studentData = reader.ReadLine().Split(';');

                    studentRepository.SaveOrUpdate
                    (
                        new Student()
                        {
                            Name = studentData[0],
                            Email = studentData[1],
                            Password = Encoding.UTF8.GetString(md5.ComputeHash(Encoding.UTF8.GetBytes(studentData[2])))
                        }
                    );
                }
            }
        }

        //[Transaction]
        public void CreateExamResults()
        {
            var examsIds = new[] { 0, 1, 3, 2, 4, 5, 6, 7, 8 };

            using (var reader = System.IO.File.OpenText(Server.MapPath("~/App_Data/notas.txt")))
            {
                while (!reader.EndOfStream)
                {
                    var data = reader.ReadLine().Split(';');
                    var studentId = int.Parse(data[0]);

                    for (int i = 1; i < 9; i++)
                    {
                        examResultRepository.SaveOrUpdate
                        (
                            new ExamResult()
                            {
                                StudentId = studentId,
                                ExamId = examsIds[i],
                                Value = double.Parse(data[i], new CultureInfo("pt-BR"))
                            }
                        );
                    }
                }
            }
        }

        #endregion
    }
}