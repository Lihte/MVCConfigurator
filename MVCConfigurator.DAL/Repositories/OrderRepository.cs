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
    public class OrderRepository : IOrderRepository
    {
        private ConfiguratorContext _context;

        public OrderRepository()
        {
            _context = new ConfiguratorContext();
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
            _context.SaveChanges();
            return order;
        }

        public bool UpdateOrder(Order order)
        {
            var exisitingOrder = GetOrderById(order.Id);
            if(exisitingOrder != null)
            {
                exisitingOrder = order;
                _context.SaveChanges();
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
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public IEnumerable<Order> GetOrdersByCustomer(int id)
        {
            return _context.Orders.Where(c => c.User.Id == id);
        }
    }
}
