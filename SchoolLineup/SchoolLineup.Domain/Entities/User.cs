﻿namespace SchoolLineup.Domain.Entities
{
    using SchoolLineup.Domain.Enums;

    public class User : EntityWithId
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public UserProfile Profile { get; set; }
    }
}