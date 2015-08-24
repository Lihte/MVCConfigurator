using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace MVCConfigurator.Domain.Models
    {
    public class User:IPrincipal
        {
        public int Id { get; set; }
        public byte[] Password { get; set; }
        public Role Role { get; set; }
        public string UserName { get; set; }


        public IIdentity Identity
            {
            get { return new GenericIdentity(UserName); }
            }

        public bool IsInRole(string role)
            {
            throw new NotImplementedException();
            }
        }
    }
