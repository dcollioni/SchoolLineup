namespace SchoolLineup.Util
{
    using System.Net.Mail;

    public class Validation
    {
        public static bool IsEmailValid(string email)
        {
            var isValid = true;

            try
            {
                new MailAddress(email);
            }
            catch
            {
                isValid = false;
            }

            return isValid;
        }
    }
}