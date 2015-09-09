using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MVCConfigurator.Domain.Repositories;
using MVCConfigurator.Domain.Models;

namespace MVCConfigurator.DAL.Repositories
{
    public class FakeUserRepository : IUserRepository
    {
        private static List<User> users = new List<User>() {
            new User{ Id = 1, IsAdmin = true, UserName = "Tomas"}
        };
        public User GetByUsername(string username)
        {
            return users.FirstOrDefault(u => u.UserName == username);
        }
        public User CreateUser(string username, byte[] passwordSalt, byte[] passwordHash, UserDetails userDetails)
        {
            var user = new User()
            {
                Hash = passwordHash,
                Salt = passwordSalt,
                UserName = username,
                Id = users.Count() + 1,
                IsAdmin = true,
            };

            users.Add(user);
            return user;
        }

        public User UpdateUser(User user)
        {
            var existingUser = GetByUsername(user.UserName);
            if (existingUser != null)
            {
                existingUser = user;
            }
            return existingUser;
        }
    }
}
