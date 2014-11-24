namespace SchoolLineup.Domain.Entities
{
    using System;

    public class ExamResult : EntityWithId
    {
        public int ExamId { get; set; }
        public int StudentId { get; set; }
        public double Value { get; set; }
        public string Description { get; set; }
    }
}