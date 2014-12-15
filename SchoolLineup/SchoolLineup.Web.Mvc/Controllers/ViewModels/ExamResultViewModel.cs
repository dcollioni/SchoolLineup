namespace SchoolLineup.Web.Mvc.Controllers.ViewModels
{
    using System;

    public class ExamResultViewModel
    {
        public int Id { get; set; }
        public double Value { get; set; }
        public string Description { get; set; }

        public int ExamId { get; set; }
        public string ExamName { get; set; }
        public string ExamDescription { get; set; }
        public double ExamValue { get; set; }
        public DateTime ExamDate { get; set; }
        public string ExamDateStr { get; set; }

        public int StudentId { get; set; }
        public string StudentName { get; set; }
    }
}