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

        private static List<Product> _products = new List<Product>()
        { 
            new Product{Id = 1, Category = new ProductCategory{ Id =10, Name="Bilar"}, Name ="Volvo V70", 

                Parts = new List<Part>()
                {
                    new Part{ Id = 100, Name="Hjul_1", Price=500, LeadTime=10, ImagePath="\\Temp", StockKeepingUnit="B1H1"},
                    new Part{ Id = 200, Name="Hjul_2", Price=700, LeadTime=20, ImagePath="\\Temp", StockKeepingUnit="B1H2"},
                    new Part{ Id = 101, Name="Fälg_3", Price=500, LeadTime=10, ImagePath="\\Temp", StockKeepingUnit="B1F1"},
                    new Part{ Id = 201, Name="Fälg_4", Price=700, LeadTime=20, ImagePath="\\Temp", StockKeepingUnit="B1F2"}
                }, 
                ImagePath="\\Products", ProductCode=""
            },
            
            new Product{Id = 2, Category = new ProductCategory{ Id =11, Name="Cyklar"}, Name ="Crescent", 

                Parts = new List<Part>()
                {
                    new Part{ Id = 101, Category = new PartCategory{ Id=1, Name="Hjul"},  Name="Däck_1", Price=300, LeadTime=12, ImagePath="\\Temp", StockKeepingUnit="C1D1"},
                    new Part{ Id = 202, Category = new PartCategory { Id=2, Name="Fälgar"}, Name="Däck_2", Price=200, LeadTime=22, ImagePath="\\Temp", StockKeepingUnit="C1D2"}

                }, ImagePath="\\Products", ProductCode=""
                }       
        };

        private static List<ProductCategory> _productCategories = new List<ProductCategory>
        {
            new ProductCategory{ Id =10, Name="Bilar" },
            new ProductCategory { Id =11, Name="Cyklar" }
        };
        private static List<PartCategory> _partCategories = new List<PartCategory>
        {
            new PartCategory{ Id=1, Name="Hjul"},
            new PartCategory { Id=2, Name="Fälgar"}
        };
        public Product AddProduct(Product product)
            {
            _products.Add(product);
            AddCategory(product.Category);
            return product;
            }

        private void AddCategory(ProductCategory category)
            {
            var exists =false;

            foreach(var item in _productCategories)
                {
                if(item.Id == category.Id || item.Name == category.Name)
                    {
                    exists = true;
                    }
                }

            if(!exists)
                _productCategories.Add(category);
            }


        public Product GetProduct(int id)
            {
            return _products.FirstOrDefault(x => x.Id==id);
            }
        public IList<Product> GetAllProducts()
            {
            return _products;
            }

        public IList<Product> GetProductsByCategory(ProductCategory category)
            {
            var list = _products.Where(p => p.Category.Id == category.Id).ToList();
            return list;

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

        public IEnumerable<ProductCategory> GetAllProductCategories()
            {
            return _productCategories;
            }
        }
    }
