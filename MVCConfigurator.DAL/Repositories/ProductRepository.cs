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
        private readonly ConfiguratorDbContext _context;
        public ProductRepository(ConfiguratorDbContext context)
        {
            _context = context;
        }

        public Product Add(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
            return product;
        }
        public Product Get(int id)
        {
            return _context.Products.FirstOrDefault(x => x.Id == id);
        }
        public IEnumerable<Product> GetAll()
        {
            return _context.Products;
        }
        public void Delete(Product product)
        {
            var prod = Get(product.Id);

            if (prod != null)
            {
                _context.Products.Remove(prod);
                _context.SaveChanges();
            }
        }
        public void Update(Product product)
        {
            var prod = Get(product.Id);

            if (prod != null)
            {
                prod = product;
                _context.SaveChanges();
            }
        }

        public bool UpdateProduct(Product product, Part part)
        {
            var prod = _context.Products.FirstOrDefault(x => x.Id == product.Id);

            if (prod != null)
            {
                prod.Parts.SingleOrDefault(p => p.Id == part.Id).IncompatibleParts.Clear();

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
