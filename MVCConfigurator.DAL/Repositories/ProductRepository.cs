using MVCConfigurator.Domain.Repositories;
using MVCConfigurator.Domain.Models;
using System.Data.Entity;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace MVCConfigurator.DAL.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private ConfiguratorContext _context;
        public ProductRepository()
        {
            _context = new ConfiguratorContext();
        }
        
        public Product AddProduct(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
            return product;
        }

        public Product GetProduct(int id)
        {
            return _context.Products.FirstOrDefault(x => x.Id == id);
        }

        public IList<Product> GetAllProducts()
        {
            return _context.Products.ToList();
        }

        public IList<Product> GetProductsByCategory(ProductCategory category)
        {
            return _context.Products.Where(x => x.Category.Id == category.Id).ToList();
        }

        public bool DeleteProduct(Product product)
        {
            var prod = _context.Products.FirstOrDefault(x => x.Id == product.Id);

            if(prod != null)
            {
                _context.Products.Remove(prod);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public bool UpdateProduct(Product product)
        {
            var prod = _context.Products.FirstOrDefault(x => x.Id == product.Id);

            if(prod != null)
            {
                prod = product;
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public IEnumerable<ProductCategory> GetAllProductCategories()
        {
            return _context.ProductCategories;
        }
    }
}
