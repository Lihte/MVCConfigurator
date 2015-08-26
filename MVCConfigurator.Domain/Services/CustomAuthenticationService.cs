using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using MVCConfigurator.Domain.Repositories;
using MVCConfigurator.Domain.Handlers;
using MVCConfigurator.Domain.Models;

namespace MVCConfigurator.Domain.Services
{
    public class CustomAuthenticationService : ICustomAuthenticationService
    {
        private IPasswordHandler _passwordHandler;
        private IUserRepository _userRepository;
        private IAuth _adapter;
        public CustomAuthenticationService(IUserRepository userRepository, IPasswordHandler passwordHandler,IAuth adapter)
        {
            _userRepository = userRepository;
            _passwordHandler = passwordHandler;
            _adapter = adapter;
        }
        public bool Login(string username, string password)
        {
            User user = _userRepository.GetByUsername(username);
            var IsCorrect = _passwordHandler.Validate(password, user.Salt, user.Hash);

            if(IsCorrect)
                _adapter.DoAuth(username);

            return IsCorrect;
        }

        public User RegisterUser(string username, string password)
        {
            byte[] salt;
            byte[] hash;

            _passwordHandler.SaltAndHash(password, out salt, out hash);

            return _userRepository.CreateUser(username, salt, hash);
        }

        public User AuthenticateRequest(System.Web.HttpContextBase httpContext)
        {
            if(httpContext.Request.Cookies["userName"]==null)
                return null;
            var userName = httpContext.Request.Cookies["userName"].Value;
            var user = _userRepository.GetByUsername(userName);
            httpContext.User = user;

            return user;

        }
    }
}
