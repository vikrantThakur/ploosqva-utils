using System.Net;
using System.Net.Mail;

namespace Ploosqva.WebUtils.Communication
{
    /// <summary>
    /// Used to send email via a remote SMTP server
    /// </summary>
    [Castle.Core.Transient]
    public class NetworkSmtpEmailSender : EmailSenderBase
    {
        ///<summary>
        /// Creates a new instance of NetworkSmtpEmailSender
        ///</summary>
        public NetworkSmtpEmailSender(string smtpUser, string smtpPass, string smtpHost, int smtpPort, bool requireSsl)
        {
            smtpClient = new SmtpClient(smtpHost, smtpPort)
                             {
                                 EnableSsl = requireSsl,
                                 DeliveryMethod = SmtpDeliveryMethod.Network,
                                 Credentials = new NetworkCredential(smtpUser, smtpPass)
                             };
        }
    }
}
