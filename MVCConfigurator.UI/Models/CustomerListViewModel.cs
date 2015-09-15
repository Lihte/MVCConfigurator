using MVCConfigurator.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCConfigurator.UI.Models
{
    public class CustomerListViewModel
    {
        public IList<User> Users { get; set; } 

    }
}