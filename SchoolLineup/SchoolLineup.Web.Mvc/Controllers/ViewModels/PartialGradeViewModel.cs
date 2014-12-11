namespace SchoolLineup.Web.Mvc.Controllers.ViewModels
{
    using System;

    public class PartialGradeViewModel
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }
    }
}