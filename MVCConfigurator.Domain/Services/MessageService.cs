using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace MVCConfigurator.Domain.Services
{
    public class MessageService : IMessageService
    {
        public void SendMail(string subject, string body, string mailAddress)
        {
            SmtpClient client = new SmtpClient();
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential();
            MailAddress from = new MailAddress("fredrik.boethius@luthman.se");

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
