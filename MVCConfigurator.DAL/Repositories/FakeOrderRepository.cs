using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MVCConfigurator.Domain.Repositories;
using MVCConfigurator.Domain.Models;

namespace MVCConfigurator.DAL.Repositories
    {
    public class FakeOrderRepository:IOrderRepository
        {
        private static List<Order> _orderList = new List<Order>()
        {
        new Order{ Id = 101, Product = new Product{Id = 2, Category = new Category{ Id =11, Name="Cyklar"}, Name ="Crescent", 

                Parts = new List<List<Part>>
                {
                    new List<Part>{
                    new Part{ Id = 101, Name="Däck_1", Price=300, LeadTime=12, ImagePath="\\Temp", StockKeepingUnit="C1D1"},
                    new Part{ Id = 202, Name="Däck_2", Price=200, LeadTime=22, ImagePath="\\Temp", StockKeepingUnit="C1D2"}
                    }
                }, ImagePath="\\Products", ProductCode=""
                }, DeliveryDate = DateTime.Now.AddDays(5), IsReady =false, Price =100  
                },
                
                new Order{ Id = 102, Product = new Product{Id = 2, Category = new Category{ Id =11, Name="Cyklar"}, Name ="Crescent", 

                Parts = new List<List<Part>>
                {
                    new List<Part>{
                    new Part{ Id = 101, Name="Däck_1", Price=300, LeadTime=12, ImagePath="\\Temp", StockKeepingUnit="C1D1"},
                    new Part{ Id = 202, Name="Däck_2", Price=200, LeadTime=22, ImagePath="\\Temp", StockKeepingUnit="C1D2"}
                    }
                }, ImagePath="\\Products", ProductCode=""
                }, DeliveryDate = DateTime.Now.AddDays(5), IsReady =false, Price =100  
                }
        };
        public IList<Order> GetAllOrders()
            {
            return _orderList;
            }

        public Order GetOrderById(int orderId)
            {
            return _orderList.FirstOrDefault(o=>o.Id==orderId);
            }

        public IList<Order> GetOrderByProduct(Product product)
            {
            throw new NotImplementedException();
            }

        public Order AddOrder(Order order)
            {
            _orderList.Add(order);
            return order;
            }

        public bool UpdateOrder(Order order)
            {
            var existingOrder=_orderList.FirstOrDefault(o => o.Id==order.Id);

            if(existingOrder!=null)
                {
                existingOrder = order;
                return true;
                }
            return false;
            }

        public bool DeleteOrder(Order order)
            {
            var existingOrder = _orderList.FirstOrDefault(o=>o.Id == order.Id);
            if(existingOrder!=null)
                {
                _orderList.Remove(order);
                return true;
                }
            return false;
            
            }
        }
    }
