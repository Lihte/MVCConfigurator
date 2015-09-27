using MVCConfigurator.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MVCConfigurator.Domain.Models;
using System.Data.Entity;

namespace MVCConfigurator.DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ConfiguratorContext _context;
        public UserRepository (ConfiguratorContext context)
	    {
            _context = context;
	    }
        public User GetByUsername(string username)
        {
            return _context.Users.FirstOrDefault(x => x.UserName == username);
        }

        public User CreateUser(string username, byte[] passwordSalt, byte[] passwordHash, UserDetails userDetails)
        {
            var user = new User()
            {
                Hash = passwordHash,
                Salt = passwordSalt,
                UserName = username,
                IsAdmin = false,
                UserDetails = userDetails
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            return user;
        }
        public User UpdateUser(User user)
        {
            var existingUser = GetByUsername(user.UserName);
            if(existingUser != null)
            {
                existingUser = user;
                _context.SaveChanges();
            }
            return existingUser;
        }


        public IList<User> GetAllUsers()
        {
            return _context.Users.ToList();
        }
    }
}
