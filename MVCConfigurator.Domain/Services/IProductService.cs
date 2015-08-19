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
        bool UpdateProduct(int id);
        bool DeleteProduct(Product product);
        IList<Part> DisplayParts();

        }
    }
