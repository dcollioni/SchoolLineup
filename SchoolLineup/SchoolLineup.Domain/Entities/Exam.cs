namespace SchoolLineup.Domain.Entities
{
    using System;

    public class Exam : EntityWithId
    {
        public int PartialGradeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Value { get; set; }
        public DateTime Date { get; set; }
    }
}