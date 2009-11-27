using System.Web.Security;

namespace Ploosqva.WebUtils.Security
{
    /// <summary>
    /// Used to create hashes of diffenrent values
    /// </summary>
    public class PasswordHasher
    {
        /// <summary>
        /// Creates an MD5 hash of the password
        /// </summary>
        /// <param name="pass">password string</param>
        public static string HashPassword(string pass)
        {
            return FormsAuthentication.HashPasswordForStoringInConfigFile(pass, "MD5");
        }
    }
}
