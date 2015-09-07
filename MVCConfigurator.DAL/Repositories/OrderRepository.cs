using MVCConfigurator.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MVCConfigurator.Domain.Models;
using System.Data.Entity;

namespace MVCConfigurator.DAL.Repositories
{
    //public class OrderContext : DbContext
    //{
    //    public DbSet<Order> Orders { get; set; }
    //}

    public class OrderRepository : IOrderRepository
    {
        private ConfiguratorContext _context;

        public OrderRepository()
        {
            _context = new ConfiguratorContext();
        }

        public IEnumerable<Order> GetAllOrders()
        {
            return _context.Orders;
        }

        public Order GetOrderById(int orderId)
        {
            return _context.Orders.FirstOrDefault(x => x.Id == orderId);
        }

        public IEnumerable<Order> GetOrdersByProduct(Product product)
        {
            return _context.Orders.Where(x => x.Product.Id == product.Id);
        }

        public Order AddOrder(Order order)
        {
            _context.Orders.Add(order);
            return order;
        }

        public bool UpdateOrder(Order order)
        {
            var exisitingOrder = GetOrderById(order.Id);
            if(exisitingOrder != null)
            {
                exisitingOrder = order;
                return true;
            }
            return false;
        }

        public bool DeleteOrder(Order order)
        {
            var exisitingOrder = GetOrderById(order.Id);
            if(exisitingOrder != null)
            {
                _context.Orders.Remove(exisitingOrder);
                return true;
            }
            return false;
        }
    }
}
