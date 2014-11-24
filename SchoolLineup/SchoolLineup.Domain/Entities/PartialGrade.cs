namespace SchoolLineup.Domain.Entities
{
    public class PartialGrade : EntityWithId
    {
        public int CourseId { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }
    }
}