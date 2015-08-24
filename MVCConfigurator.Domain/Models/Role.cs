using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MVCConfigurator.Domain.Models
    {
    public class Role
        {
        public string Description { get; set; }
        public int Id { get; set; }
        public List<Permission> Permissions { get; set; }

        }
    }
