namespace SchoolLineup.Domain.Entities
{
    public class Student : Stakeholder
    {
        public string RegistrationCode { get; set; }
        public string Password { get; set; }
    }
}