namespace SchoolLineup.Web.Mvc.Controllers
{
    using SchoolLineup.Domain.Contracts.Tasks;
    using SchoolLineup.Util;
    using System.Web.Mvc;

    public class UserController : BaseController
    {
        private readonly IUserTasks userTasks;

        public UserController(IUserTasks userTasks)
        {
            this.userTasks = userTasks;
        }

        public ActionResult Login()
        {
            if (UserLogged != null)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        public ActionResult Login(string email, string password)
        {
            password = MD5Helper.GetHash(password);

            var user = userTasks.Get(email, password);

            if (user == null)
            {
                ViewBag.Error = "Dados de acesso inválidos";
                return Redirect("/Login");
            }

            UserLogged = user;

            return RedirectToAction("Index", "Home");
        }

        //[Transaction]
        //public void Create()
        //{
        //    MD5Cng md5 = new MD5Cng();

        //    var user = new User()
        //    {
        //        Email = "email@gmail.com",
        //        Password = Encoding.Unicode.GetString(md5.ComputeHash(Encoding.Unicode.GetBytes("123456")))
        //    };

        //    userRepository.SaveOrUpdate(user);
        //}
    }
}