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
        IList<Product> GetProductsByCategory(Category category);
        bool UpdateProduct(Product product);
        bool DeleteProduct(Product product);
        IList<Part> DisplayPartsByIndex(Product product, int index);
        IList<Category> GetAllCategories();
    }
}
