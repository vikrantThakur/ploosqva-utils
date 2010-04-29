using System;
using System.IO;
using System.Net.Mail;

namespace Ploosqva.WebUtils.Communication
{
    public abstract class EmailSenderBase : IEmailSender
    {
        protected SmtpClient smtpClient;
        protected MailMessage message = new MailMessage();

        #region Implementation of IEmailSender

        Exception lastException;

        bool isBodyHtml = true;

        public Exception LastException
        {
            get { return lastException; }
        }

        public bool Send()
        {
            try
            {
                smtpClient.Send(message);
            }
            catch (Exception ex)
            {
                lastException = ex;
                return false;
            }

            return true;
        }

        public void AddAttachment(Stream attachmentStream, string name, string contentType)
        {
            message.Attachments.Add(new Attachment(attachmentStream, name, contentType));
        }

        public void AddRecipient(string recipientAddress, string recipientName)
        {
            message.To.Add(new MailAddress(recipientAddress, recipientName));
        }

        public string Sender
        {
            get
            {
                return message.From.Address;
            }
            set
            {
                message.From = new MailAddress(value);
            }
        }

        public string Body
        {
            get
            {
                return message.Body;
            }
            set
            {
                message.Body = value;
            }
        }

        public bool IsBodyHtml
        {
            get { return isBodyHtml; }
            set { isBodyHtml = value; }
        }

        public string Subject
        {
            get
            {
                return message.Subject;
            }
            set
            {
                message.Subject = value;
            }
        }

        #endregion
    }
}