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
        IList<Product> GetProductByCategory(Category category);
        bool UpdateProduct(int id);
        bool DeleteProduct(Product product);

        }
    }
