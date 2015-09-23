using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MVCConfigurator.Domain.Services;
using MVCConfigurator.DAL.Repositories;
using MVCConfigurator.Domain.Models;

namespace MVCConfiguratorTest
{
    [TestClass]
    public class TestOrderService
    {

        [TestMethod]
        public void AssertThatGetAllOrdersDoesNotReturnNullList()
        {
            Assert.IsTrue(_service.GetOrdersByCustomer(1).Count > 0);
        }

        [TestMethod]
        public void AssertThatGetOrderByIdReturnsCorrectOrder()
        {
            Assert.AreEqual(101, _service.GetOrderById(101).Id);
        }

        [TestMethod]
        public void AssertThatAddOrderReturnsAddedOrder()
        {
            var order = new Order() { Id = 103, Price = 343, IsReady = false, DeliveryDate = DateTime.Now.AddDays(10), Product = new Product() { Id = 1 } };

            Assert.AreSame(order, _service.AddOrder(order));
        }

        [TestMethod]
        public void AssertThatUpdateOrderReturnsTrue()
        {
            var order = _service.GetOrderById(102);

            order.DeliveryDate = DateTime.Now.AddDays(10);

            Assert.IsTrue(DateTime.Now.AddDays(10) == order.DeliveryDate);
        }
    }
}
