using MVCConfigurator.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCConfigurator.UI.Models
{
    public class ProductViewModel
    {
        public string Name { get; set; }
        public string ProductCode { get; set; }
        public string ImagePath { get; set; }
        public IList<Part> Parts { get; set; }
        
    }
}