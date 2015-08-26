using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace MVCConfigurator.Domain.Models
{
    public class User
    {
        public int Id { get; set; }
        public byte[] Salt { get; set; }
        public byte[] Hash { get; set; }
        public bool IsAdmin { get; set; }
        public string UserName { get; set; }

    }
}
