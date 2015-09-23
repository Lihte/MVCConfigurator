using MVCConfigurator.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using MVCConfigurator.Domain.Services;

namespace MVCConfigurator.UI.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private IUserService _userService;
        private IMessageService _messageService;

        public AuthenticationService(IUserService userService, IMessageService messageService)
        {
            _userService = userService;
            _messageService = messageService;
        }
        public void AuthenticateRequest(HttpContextBase context)
        {
            var cookieName = FormsAuthentication.FormsCookieName;
            var authCookie = context.Request.Cookies[cookieName];

            if(null==authCookie)
            {
                return;
            }

            FormsAuthenticationTicket authTicket = null;
            authTicket = FormsAuthentication.Decrypt(authCookie.Value);

            if(null == authTicket)
            {
                return;
            }

            context.User = GetPricipal(authTicket);
        }

        private User GetPricipal(FormsAuthenticationTicket authTicket)
        {
            var userName = authTicket.Name;
            var resourceId = 0;

            Int32.TryParse(authTicket.UserData, out resourceId);

            return _userService.Get(userName).Entity;
        }

        public void LoginUser(Domain.Models.User user, HttpContextBase context, bool isPersistent)
        {
            FormsAuthenticationTicket authTicket = CreateAuthenticationTicket(user,isPersistent,context);
            string encryptedTicket = FormsAuthentication.Encrypt(authTicket);

            if(context != null)
            {
                var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                authCookie.Expires = authTicket.Expiration;
                authCookie.HttpOnly = true;

                context.Response.Cookies.Add(authCookie);
                context.User = user;
            }
        }

        private static FormsAuthenticationTicket CreateAuthenticationTicket(User user, bool isPersistent, HttpContextBase context)
        {
            var expirationDate = isPersistent 
                ? DateTime.Now.AddDays(30)
                : DateTime.Now.AddMinutes(
                (context == null || context.Session == null) ? 20 : context.Session.Timeout);

            var ticket = new FormsAuthenticationTicket(
                1,
                user.UserName,
                DateTime.Now,
                expirationDate,
                isPersistent,
                string.Empty);

            return ticket;
        }

        public void LogOut()
        {
            FormsAuthentication.SignOut();
        }

        public void ResetPassword(string username)
        {
            //var user = _userService.Get(username);
            if(_userService.Get(username).Entity!=null)
            {
                var user = _userService.Get(username);
                user.Entity.RequestPasswordToken = Guid.NewGuid();

                var resetPasswordLink = "/Home/CreateNewPassword?token="+user.Entity.RequestPasswordToken.ToString();
                _messageService.SendMail("Reset password request", resetPasswordLink, user.Entity.UserName);

                _userService.UpdateUser(user.Entity);
            }

            
        }
    }
}