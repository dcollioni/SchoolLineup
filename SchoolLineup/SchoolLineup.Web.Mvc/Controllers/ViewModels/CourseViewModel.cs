namespace SchoolLineup.Web.Mvc.Controllers.ViewModels
{
    using System;

    public class CourseViewModel
    {
        public int Id { get; set; }
        public int CollegeId { get; set; }
        public string CollegeName { get; set; }
        public int TeacherId { get; set; }
        public string TeacherName { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime FinishDate { get; set; }
        public string StartDateStr { get; set; }
        public string FinishDateStr { get; set; }
        public bool IsClosed { get; set; }
    }
}