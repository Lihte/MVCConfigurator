using MVCConfigurator.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCConfigurator.UI.Models
{
    public class ProductListViewModel
    {
        public IList<Product> Products { get; set; }

        public IList<ProductViewModel> ProductViewModels { get; set; }
    }
}