using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MVCConfigurator.Domain.Models;

namespace MVCConfigurator.Domain.Repositories
{
    public interface IProductRepository : IRepository<Product>
    {
        bool UpdateProduct(Product product, Part part);
        IEnumerable<ProductCategory> GetAllProductCategories();
    }
}
