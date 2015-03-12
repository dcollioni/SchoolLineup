namespace SchoolLineup.Web.Mvc.Controllers.ViewModels
{
    using SchoolLineup.Domain.Enums;

    public class UserViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public UserProfile Profile { get; set; }
    }
}