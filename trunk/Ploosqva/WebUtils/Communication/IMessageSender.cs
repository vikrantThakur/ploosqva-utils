using System.IO;
using System.Net.Mail;

namespace Ploosqva.WebUtils.Communication
{
    /// <summary>
    /// Defines methods for sending email messages
    /// </summary>
    public interface IMessageSender
    {
        /// <summary>
        /// If sending fails, exception message is stored here
        /// </summary>
        string LastExceptionMessage { get; }

        /// <summary>
        /// If sending fails, exception stack trace is stored here
        /// </summary>
        string LastExceptionStackTrace { get; }

        ///<summary>
        /// Sends an email message
        ///</summary>
        ///<param name="msg">email message to send</param>
        ///<returns>true if sending is successful</returns>
        bool Send(MailMessage msg);

        /// <summary>
        /// Sends a mail message with attachments
        /// </summary>
        /// <param name="msg">email message to send</param>
        /// <param name="attachmentStreams">array of attachment streams</param>
        /// <returns>true if sending is successful</returns>
        bool SendWithAttachments(MailMessage msg, Stream[] attachmentStreams);
    }
}
