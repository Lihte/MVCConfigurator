using MVCConfigurator.Domain.Models;
using MVCConfigurator.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MVCConfigurator.UI.Services
{
    public interface IAuthenticationService
    {
        void AuthenticateRequest(HttpContextBase context);
        void LoginUser(User user,HttpContextBase context, bool isPersistent);
        void LogOut();
        void ResetPassword(string username);
    }
}
