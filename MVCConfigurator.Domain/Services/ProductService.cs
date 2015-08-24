using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MVCConfigurator.Domain.Models;
using MVCConfigurator.Domain.Repositories;


namespace MVCConfigurator.Domain.Services
{
    public class ProductService : IProductService
    {
        private IProductRepository _repository;
        public ProductService(IProductRepository repository)
        {
            _repository = repository;
        }
        public Product AddProduct(Product product)
        {
            _repository.AddProduct(product);
            return product;
        }

        public Product GetProduct(int id)
        {
            return _repository.GetProduct(id);
        }

        public IList<Product> GetAllProducts()
        {
            return _repository.GetAllProducts();
        }

        public IList<Product> GetProductsByCategory(ProductCategory category)
        {
            return _repository.GetProductsByCategory(category);
        }

        public bool UpdateProduct(Product product)
        {
            return _repository.UpdateProduct(product);
        }

        public bool DeleteProduct(Product product)
        {
            return _repository.DeleteProduct(product);
        }

        public IList<Part> DisplayPartsByCategory(Product product, PartCategory category)
        {
            return product.Parts.Where(p => p.Category.Id == category.Id).ToList();
        }
        public IList<ProductCategory> GetAllProductCategories()
        {

            return _repository.GetAllProductCategories().ToList();
        }
    }
}
