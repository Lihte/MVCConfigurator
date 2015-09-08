using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace MVCConfigurator.Domain.Models
{
    public class User:IPrincipal
    {
        public User()
        {
            UserDetails = new UserDetails();
        }

        public int Id { get; set; }
        public UserDetails UserDetails { get; set; }
        public byte[] Salt { get; set; }
        public byte[] Hash { get; set; }
        public bool IsAdmin { get; set; }
        public string UserName { get; set; }
        public Guid RequestPasswordToken { get; set; }
        public virtual IList<Order> Orders { get; set; }
        public IIdentity Identity
        {
            get { return new GenericIdentity(UserName); }
        }

        public bool IsInRole(string role)
        {
            return IsAdmin;
        }
    }

    public class UserDetails
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Company { get; set; }
    }
}
