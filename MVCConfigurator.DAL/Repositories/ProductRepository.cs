using MVCConfigurator.Domain.Repositories;
using MVCConfigurator.Domain.Models;
using System.Data.Entity;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace MVCConfigurator.DAL.Repositories
{
    public class ProductContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
    }
    public class ProductRepository : IProductRepository
    {
        private ProductContext _context;
        public ProductRepository()
        {
            _context = new ProductContext();
        }
        
        
        public Product AddProduct(Product product)
        {
            _context.Products.Add(product);
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
