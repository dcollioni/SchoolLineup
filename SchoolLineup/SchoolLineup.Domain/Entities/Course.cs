namespace SchoolLineup.Domain.Entities
{
    using System;

    public class Course : EntityWithId
    {
        public int CollegeId { get; set; }
        public int TeacherId { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime FinishDate { get; set; }
        public bool IsClosed { get; set; }
    }
}