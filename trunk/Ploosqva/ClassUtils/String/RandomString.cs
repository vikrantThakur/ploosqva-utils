using System;
using System.Text;

namespace Ploosqva.ClassUtils.String
{
    ///<summary>
    /// Represents a random string 
    ///</summary>
    public class RandomString
    {
        private string allowedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";

        /// <summary>
        /// String containing characters allowed in the random string.
        /// By default it is equvialent to [a-zA-Z0-9] regular expression
        /// </summary>
        public string AllowedChars
        {
            get
            {
                return allowedChars;
            }
            set
            {
                allowedChars = value;
            }
        }

        ///<summary>
        /// Random string generated
        ///</summary>
        public string Value { get; private set; }

        ///<summary>
        /// Creates a string containing random characters from the set 
        /// AllowedChars property
        ///</summary>
        ///<param name="length"></param>
        public RandomString(int length)
        {
            StringBuilder sBuilder = new StringBuilder(length);
            Random rand = new Random();
            int allowedNum = allowedChars.Length;

            for (int i = 0; i < length; i++)
            {
                char nextChar = allowedChars[rand.Next(allowedNum)];
                sBuilder.Append(nextChar);
            }

            Value = sBuilder.ToString();
        }
    }
}
