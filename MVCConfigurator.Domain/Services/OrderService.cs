using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MVCConfigurator.Domain.Models;
using MVCConfigurator.Domain.Repositories;

namespace MVCConfigurator.Domain.Services
{
    public class OrderService : IOrderService
    {
        private IOrderRepository _repository;

        public OrderService(IOrderRepository repository)
        {
            _repository = repository;
        }

        public Order AddOrder(Order order)
        {
            return _repository.Add(order);
        }
        public Order GetOrder(int id)
        {
            return _repository.Get(id);
        }
        public void UpdateOrder(Order order)
        {
            _repository.Update(order);
        }
        public void DeleteOrder(Order order)
        {
            _repository.Delete(order);
        }

        public IList<Order> GetOrdersByCustomer(int id)
        {
            return _repository.GetOrdersByCustomer(id).ToList();
        }
    }
}
