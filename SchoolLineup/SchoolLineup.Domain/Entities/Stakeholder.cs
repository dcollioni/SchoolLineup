namespace SchoolLineup.Domain.Entities
{
    public abstract class Stakeholder : EntityWithId
    {
        public string Name { get; set; }
        public string Email { get; set; }
    }
}