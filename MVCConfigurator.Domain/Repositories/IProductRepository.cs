using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MVCConfigurator.Domain.Models;

namespace MVCConfigurator.Domain.Repositories
{
    public interface IProductRepository
    {
        Product AddProduct(Product product);
        Product GetProduct(int id);
        IList<Product> GetAllProducts();
        IList<Product> GetProductsByCategory(ProductCategory category);
        bool UpdateProduct(Product product);
        bool DeleteProduct(Product product);

        IEnumerable<ProductCategory> GetAllProductCategories();
    }
}
