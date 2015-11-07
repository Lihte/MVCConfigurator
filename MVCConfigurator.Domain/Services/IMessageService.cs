using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCConfigurator.Domain.Services
{
    public interface IMessageService
    {
        void SendMail(string subject, string body, string address);
    }
}
