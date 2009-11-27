using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Ploosqva.Security
{
    /// <summary>
    /// Class used to encrypt and decrypt using DES algorithm
    /// </summary>
    public static class DESCryptography
    {
        /// <summary>
        /// Encrypts a string using the DES algorightm and encodes as Base64 string
        /// </summary>
        public static string EnryptToBase64String(string input, byte[] key, byte[] iV)
        {
            MemoryStream mStream = new MemoryStream();

            // Create a CryptoStream using the MemoryStream 
            // and the passed key and initialization vector (IV).
            CryptoStream cStream = new CryptoStream(mStream,
                new DESCryptoServiceProvider().CreateEncryptor(key, iV),
                CryptoStreamMode.Write);

            // Convert the passed string to a byte array.
            byte[] toEncryptBytes = new ASCIIEncoding().GetBytes(input);

            // Write the byte array to the crypto stream and flush it.
            cStream.Write(toEncryptBytes, 0, toEncryptBytes.Length);
            cStream.FlushFinalBlock();

            // Get an array of bytes from the 
            // MemoryStream that holds the 
            // encrypted data.
            byte[] ret = mStream.ToArray();

            // Close the streams.
            cStream.Close();
            mStream.Close();

            return Convert.ToBase64String(ret);
        }

        /// <summary>
        /// Decrypts a Base64 string using DES algorithm. For correct output,
        /// key and iV values must same as those used to encrypt (for DES is a symmetric algorithm) 
        /// </summary>
        public static string DecryptFromBase64String(string input, byte[] key, byte[] iV)
        {
            byte[] data = Convert.FromBase64String(input);
            MemoryStream mStream = new MemoryStream(data);

            // Create a CryptoStream using the MemoryStream 
            // and the passed key and initialization vector (IV).
            CryptoStream csDecrypt = new CryptoStream(mStream,
                new DESCryptoServiceProvider().CreateDecryptor(key, iV),
                CryptoStreamMode.Read);

            // Read the decrypted data out of the crypto stream
            return new StreamReader(csDecrypt).ReadToEnd();
        }
    }
}
