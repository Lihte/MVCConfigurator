using MVCConfigurator.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCConfigurator.UI.Models
{
    public class OrderViewModel
    {
        public OrderViewModel(Order order)
        {
            Product = order.Product;
            Price = order.Price;
            DeliveryDate = order.DeliveryDate;
            IsReady = order.IsReady;
            User = order.User;
            ProductCode = order.ProductCode;
        }

        public Product Product { get; set; }
        public string ProductCode { get; set; }
        public decimal Price { get; set; }
        public DateTime DeliveryDate { get; set; }
        public User User { get; set; }
        public bool IsReady { get; set; }
    }
}