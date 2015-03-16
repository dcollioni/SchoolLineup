namespace SchoolLineup.Domain.Entities
{
    using SchoolLineup.Domain.Enums;
    using System;

    public class User : EntityWithId
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsPasswordTemp { get; set; }
        public UserProfile Profile { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}