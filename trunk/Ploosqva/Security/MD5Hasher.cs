using System;
using System.Security.Cryptography;
using System.Text;

namespace Ploosqva.Security
{
    ///<summary>
    /// Class used to use MD5 algorithm
    ///</summary>
    public class MD5Hasher
    {
        ///<summary>
        /// Hash an input string and return the hash as
        /// a 32 character hexadecimal string.
        ///</summary>
        public static string GetHash(string input)
        {
            // Create a new instance of the MD5CryptoServiceProvider object.
            MD5 md5Hasher = MD5.Create();

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        /// <summary>
        /// Verify a hash against a string.
        /// </summary>
        public static bool VerifyHash(string input, string hash)
        {
            // Hash the input.
            string hashOfInput = GetHash(input);

            // Create a StringComparer an compare the hashes.
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            return 0 == comparer.Compare(hashOfInput, hash);
        }
    }
}
