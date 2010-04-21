namespace Ploosqva.Security
{
    ///<summary>
    /// Interface definig actions to create a hash of input string
    ///</summary>
    public interface IHashService
    {
        ///<summary>
        /// Hash an input string and return the hash
        ///</summary>
        string GetHash(string input);

        /// <summary>
        /// Verify a hash against a string.
        /// </summary>
        bool VerifyHash(string input, string hash);
    }
}