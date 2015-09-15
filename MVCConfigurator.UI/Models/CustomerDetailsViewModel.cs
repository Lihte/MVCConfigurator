using MVCConfigurator.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MVCConfigurator.UI.Models
{
    public class CustomerDetailsViewModel
    {
        public List<OrderModel> Orders { get; set; }
        public User Customer { get; set; }
    }

    public class OrderModel
    {
        public int Id { get; set; }
        public bool IsReady { get; set; }
        public DateTime DeliveryDate { get; set; }
    }
}
