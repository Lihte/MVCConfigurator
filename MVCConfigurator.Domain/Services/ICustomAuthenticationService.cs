using MVCConfigurator.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MVCConfigurator.Domain.Services
    {
    public interface ICustomAuthenticationService
        {
        bool Login(string username, string password);
        User RegisterUser(string username, string password);
        User AuthenticateRequest(HttpContextBase httpContext);

        }
    }
