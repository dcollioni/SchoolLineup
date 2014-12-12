namespace SchoolLineup.Web.Mvc.Controllers.ViewModels
{
    using System;

    public class ExamViewModel
    {
        public int Id { get; set; }
        public int PartialGradeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Value { get; set; }
        public DateTime Date { get; set; }
    }
}