using MVCConfigurator.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCConfigurator.UI.Models
{
    public class CustomizeProductViewModel
    {
        public CustomizeProductViewModel()
        {
            SelectedParts = new List<PartModel>();
        }

        public ProductModel Product { get; set; }
        public IList<ProductViewModel> Products { get; set; }
        public IList<PartModel> SelectedParts { get; set; }
    }
}