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

        }
        public ProductViewModel(Product product)
        {
            Product = new ProductModel(product);
        }
        public ProductModel Product { get; set; }
        public IEnumerable<SelectListItem> Categories { get; set; }
    }
    public class ProductModel
    {

        public ProductModel()
        {

        }

        public ProductModel(Product product)
        {
            Id = product.Id;
            Category = product.Category.Name;
            ImagePath = product.ImagePath;
            ProductCode = product.ProductCode;
            Parts = new List<PartModel>();
        }

        public int Id { get; set; }
        public string Category { get; set; }
        public string ProductCode { get; set; }
        public string ImagePath { get; set; }
        public IList<PartModel> Parts { get; set; }
    }
}