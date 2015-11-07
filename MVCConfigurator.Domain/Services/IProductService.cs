using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MVCConfigurator.Domain.Models;

namespace MVCConfigurator.Domain.Services
{
    public interface IProductService
    {
        Product AddProduct(Product product);
        Product GetProduct(int id);
        IList<Product> GetAllProducts();
        void UpdateProduct(Product product);
        bool UpdateProduct(Product product, Part part);
        void DeleteProduct(Product product);
        IList<ProductCategory> GetAllProductCategories();
        IList<PartCategory> GetAllPartCategoriesByProduct(Product product);
    }
}
