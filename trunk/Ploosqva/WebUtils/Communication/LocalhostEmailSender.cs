using System.Net.Mail;

namespace Ploosqva.WebUtils.Communication
{
    /// <summary>
    /// Used to send email via a local IIS SMTP service
    /// </summary>
    [Castle.Core.Transient]
    public class LocalhostEmailSender : EmailSenderBase
    {
        private int port = 25;

        /// <summary>
        /// SmtpPort used by the smtp server. Default is 25
        /// </summary>
        public int SmtpPort
        {
            set
            {
                port = value;
            }
            get
            {
                return port;
            }
        }

        ///<summary>
        /// Creates a new SmtpClient using localhost and
        /// port set by <see cref="SmtpPort"/> property
        ///</summary>
        public LocalhostEmailSender()
        {
            smtpClient = new SmtpClient("localhost", port);
        }
    }
}
