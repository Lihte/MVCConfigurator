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
        IList<Product> GetProductsByCategory(ProductCategory category);
        bool UpdateProduct(Product product);
        bool UpdateProduct(Product product, Part part);
        bool DeleteProduct(Product product);
        IList<Part> DisplayPartsByCategory(Product product, PartCategory category);
        IList<ProductCategory> GetAllProductCategories();
        IList<PartCategory> GetAllPartCategoriesByProduct(Product product);

    }
}
