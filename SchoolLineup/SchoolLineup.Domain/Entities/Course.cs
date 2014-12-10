namespace SchoolLineup.Domain.Entities
{
    using System;
    using System.Collections.Generic;

    public class Course : EntityWithId
    {
        public int CollegeId { get; set; }
        public int TeacherId { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime FinishDate { get; set; }
        public bool IsClosed { get; set; }
        public IList<int> StudentsIds { get; set; }

        public Course()
        {
            StudentsIds = new List<int>();
        }
    }
}