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
        private readonly ConfiguratorDbContext _context;

        public OrderRepository(ConfiguratorDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Order> GetOrdersByCustomer(int id)
        {
            return _context.Orders.Where(c => c.User.Id == id);
        }

        public Order Add(Order order)
        {
            _context.Orders.Add(order);
            _context.SaveChanges();
            return order;
        }
        public Order Get(int id)
        {
            return _context.Orders.FirstOrDefault(x => x.Id == id);
        }
        public IEnumerable<Order> GetAll()
        {
            return _context.Orders;
        }
        public void Update(Order order)
        {
            var orderToUpdate = Get(order.Id);

            if (orderToUpdate != null)
            {
                orderToUpdate = order;
                _context.SaveChanges();
            }
        }
        public void Delete(Order order)
        {
            var orderToDelete = Get(order.Id);

            if (orderToDelete != null)
            {
                _context.Orders.Remove(orderToDelete);
                _context.SaveChanges();
            }
        }
    }
}
