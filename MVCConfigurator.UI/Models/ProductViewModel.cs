using MVCConfigurator.Domain.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace MVCConfigurator.UI.Models
{
    public class ProductViewModel
    {
        public ProductViewModel() { }

        public ProductViewModel(Product product)
        {
            Product = new ProductModel(product);
        }

        public ProductModel Product { get; set; }
    }
    public class ProductModel
    {
        public ProductModel() { }

        public ProductModel(Product product)
        {
            Id = product.Id;
            Category = product.Category.Name;
            Image = new Image(product.ImagePath);
            Parts = new List<PartModel>();
        }

        public int Id { get; set; }
        [Required]
        public string Category { get; set; }
        public Image Image { get; set; }
        public IList<PartModel> Parts { get; set; }
    }
    public class Image
    {
        public Image() { }

        public Image(string imagePath)
        {
            ImagePath = imagePath;
        }

        public string ImagePath { get; set; }
        public HttpPostedFileBase ImageUpload { get; set; }
    }
}