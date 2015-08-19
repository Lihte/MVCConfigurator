using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MVCConfigurator.Domain.Repositories;
using MVCConfigurator.Domain.Models;

namespace MVCConfigurator.DAL.Repositories
    {

    public class FakeProductRepository : IProductRepository
        {
        private static List<Product> _products = new List<Product> 
        { 
        new Product{Id = 1, Category = new Category{ Id =10, Name="Bilar"}, Name ="Volvo V70", 

        Parts = new List<List<Part>>
        {
            new List<Part>{
            new Part{ Id = 100, Name="Hjul_1", Price=500, LeadTime=10, ImagePath="\\Temp", StockKeepingUnit="B1H1"},
            new Part{ Id = 200, Name="Hjul_2", Price=700, LeadTime=20, ImagePath="\\Temp", StockKeepingUnit="B1H2"}}}, ImagePath="\\Products", ProductCode=""}
        };
        public Product AddProduct(Product product)
            {
            _products.Add(product);
            return product;
            }


        public Product GetProduct(int id)
            {
            return _products.FirstOrDefault(x => x.Id==id);
            }

        public IList<Product> GetAllProducts()
            {
            return _products;
            }

        public IList<Product> GetProductsByCategory(Category category)
            {
            return _products.Where(p => p.Category.Equals(category)).ToList();

            }

        public bool UpdateProduct(Product product)
            {
            
            var prod=_products.FirstOrDefault(p => p.Id==product.Id);
            
            if(prod!=null)
                {
                prod = product;
                return true;
                }
            return false;
            }

        public bool DeleteProduct(Product product)
            {
            var prod = _products.SingleOrDefault(p => p.Id==product.Id);
            if(prod!=null)
                {
                _products.Remove(product);
                return true;
                }
            return false;
            
            }
        }
    }
