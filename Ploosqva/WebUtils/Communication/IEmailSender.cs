using System;
using System.IO;

namespace Ploosqva.WebUtils.Communication
{
    /// <summary>
    /// Defines methods for sending email messages
    /// </summary>
    public interface IEmailSender
    {
        /// <summary>
        /// If sending fails, exception message is stored here
        /// </summary>
        Exception LastException { get; }

        ///<summary>
        /// Sends an email message
        ///</summary>
        ///<param name="msg">email message to send</param>
        ///<returns>true if sending is successful</returns>
        bool Send();

        /// <summary>
        /// Adds an attachement
        /// </summary>
        void AddAttachment(Stream attachmentStream, string name, string contentType);

        /// <summary>
        /// Adds a recipient to this email
        /// </summary>
        void AddRecipient(string recipientAddress, string recipientName);

        /// <summary>
        /// Gets or sets email's sender
        /// </summary>
        string Sender { set; get; }

        /// <summary>
        /// Gets or sets email's content
        /// </summary>
        string Body { get; set; }

        /// <summary>
        /// Gets or sets email html
        /// </summary>
        bool IsBodyHtml { get; set; }

        ///<summary>
        /// Gets or sets email's subject
        ///</summary>
        string Subject { get; set; }
    }
}
