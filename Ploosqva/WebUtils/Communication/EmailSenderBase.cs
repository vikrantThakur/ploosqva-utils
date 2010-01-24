using System;
using System.IO;
using System.Net.Mail;

namespace Ploosqva.WebUtils.Communication
{
    public abstract class EmailSenderBase : IMessageSender
    {
        protected SmtpClient smtpClient;
        protected string lastExceptionMessage;
        protected string lastExceptionStackTrace;

        public string LastExceptionMessage
        {
            get { return lastExceptionMessage; }
        }

        public string LastExceptionStackTrace
        {
            get { return lastExceptionStackTrace; }
        }

        public virtual bool Send(MailMessage msg)
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

        public virtual bool SendWithAttachments(MailMessage msg, Stream[] attachmentStreams)
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