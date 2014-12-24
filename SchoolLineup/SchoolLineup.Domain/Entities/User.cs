namespace SchoolLineup.Domain.Entities
{
    public class User : EntityWithId
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}