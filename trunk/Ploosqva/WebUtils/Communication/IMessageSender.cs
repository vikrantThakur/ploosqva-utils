namespace Ploosqva.WebUtils.Communication
{
    ///<summary>
    /// Interface to send a message over different media (sms, mms and such)
    ///</summary>
    public interface IMessageSender
    {
        ///<summary>
        /// Send a message to the recipient
        ///</summary>
        ///<param name="from">message sender</param>
        ///<param name="to">message receiver</param>
        ///<param name="message">message content</param>
        ///<returns>true if sending succeeds</returns>
        bool Send(string from, string to, string message);
    }
}
