namespace SchoolLineup.Util
{
    using System.Security.Cryptography;
    using System.Text;

    public class MD5Helper
    {
        public static string GetHash(string value)
        {
            MD5Cng md5 = new MD5Cng();

            return Encoding.Unicode.GetString(md5.ComputeHash(Encoding.Unicode.GetBytes(value)));
        }
    }
}