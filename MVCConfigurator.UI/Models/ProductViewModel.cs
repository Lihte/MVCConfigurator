using MVCConfigurator.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCConfigurator.UI.Models
{
    public class ProductViewModel
    {
        public ProductViewModel()
        {
            Product = new ProductModel();
        }
        public ProductModel Product { get; set; }
        public IEnumerable<SelectListItem> Categories { get; set; }
    }
    public class ProductModel
    {
        public ProductModel()
        {
            Parts = new List<Part>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string ProductCode { get; set; }
        public string ImagePath { get; set; }
        public IList<Part> Parts { get; set; }
    }
}