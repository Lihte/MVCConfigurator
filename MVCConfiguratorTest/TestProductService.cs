using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MVCConfigurator.Domain.Services;
using MVCConfigurator.Domain.Repositories;
using MVCConfigurator.DAL.Repositories;
using MVCConfigurator.Domain.Models;
using System.Collections.Generic;

namespace MVCConfiguratorTest
{
    [TestClass]
    public class TestProductService
    {
        IProductService _service = new ProductService(new FakeProductRepository());

        [TestMethod]
        public void AssertThat_AddProduct_Returns_Same_Product()
        {
            var prod = new Product { Id = 5, Category = new ProductCategory { Id = 16, Name = "Bullar" }, Name = "Fiskbulle", ImagePath = "", ProductCode = "", Parts = new List<Part>() };
            Assert.AreSame(prod, _service.AddProduct(prod));
        }

        [TestMethod]
        public void AssertThatGetProduct_Returns_Same_Product()
        {
            Assert.AreEqual(1, _service.GetProduct(1).Id);
        }

        [TestMethod]
        public void AssertThatGetProduct_DoesNotReturnNull()
        {
            Assert.IsNotNull(_service.GetProduct(1));
        }

        [TestMethod]
        public void AssertThatGetProductByCategoryReturnsCorrectProductList()
        {
            var category = new ProductCategory();
            category.Id = 10;
            category.Name = "Bilar";
            var list = _service.GetProductsByCategory(category);
            if (list.Count > 0)
            {
                foreach (var item in list)
                {
                    Assert.AreEqual(item.Category, category);
                }
            }
            else
            {
                throw new AssertFailedException();
            }
        }

        [TestMethod]
        public void AssertThatGetProductByCategoryReturnsNotNullProductList()
        {
            var category = new ProductCategory();
            category.Id = 10;
            category.Name = "Bilar";
            var list = _service.GetProductsByCategory(category);
            Assert.IsNotNull(list);
        }

        [TestMethod]
        public void AssertThatGetAllProductsDoesNotReturnEmptyList()
        {
            Assert.IsTrue(_service.GetAllProducts().Count > 0);
        }

        [TestMethod]
        public void AssertThatUpDateProductReturnsTrueOnSuccess()
        {
            var prod = _service.GetProduct(1);
            prod.Name = "Falukorv";
            Assert.IsTrue(_service.UpdateProduct(prod));
        }

        [TestMethod]
        public void AssertThatDeleteProductReturnsTrueOnSuccess()
        {
            var prod = _service.GetProduct(1);
            _service.DeleteProduct(prod);
            Assert.IsNull(_service.GetProduct(1));
        }

        //[TestMethod]
        //public void AssertThatDisplayPartsByIndexReturnsCorrectParts()
        //{
        //    var prod = _service.GetProduct(2);
        //    var parts = _service.DisplayPartsByIndex(prod, 0);
        //    Assert.IsTrue(parts.Count == 2);
        //}

        [TestMethod]
        public void AssertThatGetAllCategoriesIsNotNull()
        {
            Assert.IsNotNull(_service.GetAllProductCategories());
        }

        [TestMethod]
        public void AssertThatGetAllCategoriesIsNotEmpty()
        {
            Assert.IsTrue(_service.GetAllProductCategories().Count > 0);
        }
        [TestMethod]
        public void AssertThatGetAllPartCategoriesByProductReturnsAListOfCategories()
        {
            var product = _service.GetProduct(2);
            Assert.IsTrue(_service.GetAllPartCategoriesByProduct(product).Count>0);
        }
    }
}
