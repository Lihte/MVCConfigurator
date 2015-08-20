using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MVCConfigurator.Domain.Models;
using MVCConfigurator.Domain.Repositories;

namespace MVCConfigurator.Domain.Services
    {
    public class OrderService:IOrderService
        {
        private IOrderRepository _repository;

        public OrderService(IOrderRepository repository)
            {
            _repository = repository;
            }

        public IList<Order> GetAllOrders()
            {
            return _repository.GetAllOrders();
            }

        public Order GetOrderById(int orderId)
            {
            return _repository.GetOrderById(orderId);
            }

        public IList<Order> GetOrderByProduct(Product product)
            {
            throw new NotImplementedException();
            }

        public Order AddOrder(Order order)
            {
            return _repository.AddOrder(order);
            }

        public bool UpdateOrder(Order order)
            {
            return _repository.UpdateOrder(order);
            }

        public bool DeleteOrder(Order order)
            {
            return _repository.DeleteOrder(order);
            }
        }
    }
