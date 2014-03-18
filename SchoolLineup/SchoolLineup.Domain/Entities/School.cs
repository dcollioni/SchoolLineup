namespace SchoolLineup.Domain.Entities
{
    public class School : EntityWithId
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string ManagerName { get; set; }
    }
}