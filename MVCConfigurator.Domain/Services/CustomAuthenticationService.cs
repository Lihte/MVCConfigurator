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

        public CustomAuthenticationService(IUserRepository userRepository, IPasswordHandler passwordHandler)
        {
            _userRepository = userRepository;
            _passwordHandler = passwordHandler;
        }
        public bool Login(string username, string password)
        {
            User user = _userRepository.GetByUsername(username);
            var IsCorrect = _passwordHandler.Validate(password, user.Salt, user.Hash);
            return IsCorrect;
        }

        public User RegisterUser(string username, string password)
        {
            byte[] salt;
            byte[] hash;

            _passwordHandler.SaltAndHash(password, out salt, out hash);

            return _userRepository.CreateUser(username, salt, hash);
        }

        public Models.User AuthenticateRequest(System.Web.HttpContextBase httpContext)
        {
            throw new NotImplementedException();
        }
    }
}
