using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MVCConfigurator.Domain.Services;
using System.Web.Security;

namespace MVCConfigurator.UI.Authentication
{
    public class FormsAuthenticationAdapter:IAuth
    {
        public void DoAuth(string username)
        {
            FormsAuthentication.SetAuthCookie(username,false);
        }
    }
}