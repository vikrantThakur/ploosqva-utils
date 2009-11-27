using System;
using System.IO;
using System.Net.Mail;

namespace Ploosqva.WebUtils.Communication
{
    /// <summary>
    /// Used to send email via a local IIS SMTP service
    /// </summary>
    public class LocalhostEmailSender : IMessageSender
    {
        private readonly SmtpClient smtpClient;

        /// <summary>
        /// Port used by the smtp server. Default is 25
        /// </summary>
        public static int port = 25;
        private string lastExceptionMessage;
        private string lastExceptionStackTrace;

        public string LastExceptionMessage
        {
            get { return lastExceptionMessage; }
        }

        public string LastExceptionStackTrace
        {
            get { return lastExceptionStackTrace; }
        }

        ///<summary>
        /// Creates a new SmtpClient using localhost and
        /// port set by static <see cref="port"/> field
        ///</summary>
        public LocalhostEmailSender()
        {
            smtpClient = new SmtpClient("localhost", port);
        }

        public bool Send(MailMessage msg)
        {
            try
            {
                smtpClient.Send(msg);
            }
            catch (Exception ex)
            {
                lastExceptionMessage = ex.Message;
                lastExceptionStackTrace = ex.StackTrace;
                return false;
            }

            return true;
        }

        public bool SendWithAttachments(MailMessage msg, Stream[] attachmentStreams)
        {
            try
            {
                smtpClient.Send(msg);
            }
            catch (Exception ex)
            {
                lastExceptionMessage = ex.Message;
                lastExceptionStackTrace = ex.StackTrace;
                return false;
            }

            return true;
        }
    }
}
