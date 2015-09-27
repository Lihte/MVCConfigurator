using System.Net;
using System.Net.Mail;

namespace MVCConfigurator.Domain.Services
{
    public class MessageService : IMessageService
    {
        public void SendMail(string subject, string body, string mailAddress)
        {
            SmtpClient client = new SmtpClient();
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential();
            MailAddress from = new MailAddress("admin@mvcconfig.se");

            MailAddress to = new MailAddress(mailAddress);
            MailMessage message = new MailMessage(from, to);

            message.IsBodyHtml = true;
            message.Subject = subject;
            message.Body = body;

            client.SendCompleted+=client_SendCompleted;
            client.Send(message);
            
            client.Dispose();
        }

        void client_SendCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            int x = 0;
        }
    }
}
