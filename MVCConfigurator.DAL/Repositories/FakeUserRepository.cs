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
        private static List<User> users = 
            new List<User>
            {
                new User
                { 
                    Id=1, 
                    UserName="Tomas",  
                    Password=null,
                    Role = new Role
                    { 
                        Id=1, 
                        Description="admin", 
                        Permissions = new List<Permission>
                        {
                            new Permission
                            {
                                Id=1,
                                Description="AAA"
                            } 

                        }
                    }
                }
            };
        public User GetByUsername(string username)
        {
            throw new NotImplementedException();
        }

        public User CreateUser(string username, byte[] passwordSalt, byte[] passwordHash)
        {
            throw new NotImplementedException();
        }
    }
}
