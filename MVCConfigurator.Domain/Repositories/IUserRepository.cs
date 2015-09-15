using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MVCConfigurator.Domain.Models;

namespace MVCConfigurator.Domain.Repositories
    {
    public interface IUserRepository
        {
        User GetByUsername(string username);
        User CreateUser(string username, byte[] passwordSalt, byte[] passwordHash, UserDetails userDetails);
        User UpdateUser(User user);
        IList<User> GetAllUsers();
        }
    }
