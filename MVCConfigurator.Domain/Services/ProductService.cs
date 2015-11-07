using System.Collections.Generic;
using System.Linq;
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
            _repository.Add(product);
            return product;
        }
        public Product GetProduct(int id)
        {
            return _repository.Get(id);
        }
        public IList<Product> GetAllProducts()
        {
            return _repository.GetAll().ToList();
        }
        public void UpdateProduct(Product product)
        {
            _repository.Update(product);
        }
        public bool UpdateProduct(Product product, Part part)
        {
            return _repository.UpdateProduct(product, part);
        }
        public void DeleteProduct(Product product)
        {
            _repository.Delete(product);
        }

        public IList<ProductCategory> GetAllProductCategories()
        {

            return _repository.GetAllProductCategories().ToList();
        }
        public IList<PartCategory> GetAllPartCategoriesByProduct(Product product)
        {
            if(product.Parts==null)
            {
                return new List<PartCategory>();
            }
            return product.Parts.Select(p => p.Category).Distinct().ToList();
        }
    }
}
