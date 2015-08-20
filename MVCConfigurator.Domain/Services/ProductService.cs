using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MVCConfigurator.Domain.Models;
using MVCConfigurator.Domain.Repositories;


namespace MVCConfigurator.Domain.Services
    {
    public class ProductService:IProductService
        {
        private IProductRepository _repository;
        public ProductService(IProductRepository repository)
            {
            _repository=repository;
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

        public IList<Product> GetProductsByCategory(Category category)
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

        public IList<Part> DisplayPartsByIndex(Product product, int index)
            {
            return product.Parts[index];
            }
        public IList<Category> GetAllCategories()
            {

            return _repository.GetAllCategories().ToList();
            }
        }
    }
