using System.Net;
using System.Net.Mail;

namespace Ploosqva.WebUtils.Communication
{
    public class RemoteEmailSender : EmailSenderBase
    {
        public RemoteEmailSender(string host, int port, bool ssl, string user, string pass)
        {
            smtpClient = new SmtpClient(host, port)
                             {
                                 EnableSsl = ssl,
                                 DeliveryMethod=SmtpDeliveryMethod.Network
                             };
            smtpClient.Credentials = new NetworkCredential(user, pass);
        }
    }
}
