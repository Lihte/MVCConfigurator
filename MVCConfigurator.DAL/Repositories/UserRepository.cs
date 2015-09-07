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
    //public class UserContext : DbContext
    //{
    //    public DbSet<User> Users { get; set; }
    //}
    
    public class UserRepository : IUserRepository
    {
        private ConfiguratorContext _context;
        public UserRepository ()
	    {
            _context = new ConfiguratorContext();
	    }
        public User GetByUsername(string username)
        {
            return _context.Users.FirstOrDefault(x => x.UserName == username);
        }

        public User CreateUser(string username, byte[] passwordSalt, byte[] passwordHash)
        {
            var user = new User()
            {
                Hash = passwordHash,
                Salt = passwordSalt,
                UserName = username,
                IsAdmin = false,
            };

            _context.Users.Add(user);

            return user;
        }
        public User UpdateUser(User user)
        {
            var existingUser = GetByUsername(user.UserName);
            if(existingUser != null)
            {
                existingUser = user;
            }
            return existingUser;
        }
    }
}
