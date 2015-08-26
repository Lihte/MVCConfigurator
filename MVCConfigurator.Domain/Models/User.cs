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
        public byte[] Salt { get; set; }
        public byte[] Hash { get; set; }
        public bool IsAdmin { get; set; }
        public string UserName { get; set; }


        public IIdentity Identity
        {
            get { return new GenericIdentity(UserName); }
        }

        public bool IsInRole(string role)
        {
            return false;
        }
    }
}
