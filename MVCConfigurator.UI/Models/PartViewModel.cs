using Foolproof;
using MVCConfigurator.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCConfigurator.UI.Models
{
    public class PartViewModel
    {
        public int ProductId { get; set; }
        public PartModel PartDetails { get; set; }
        public IEnumerable<SelectListItem> Categories { get; set; }
        public IList<PartModel> ExistingParts { get; set; }
    }

    public class PartModel
    {
        public PartModel() 
        {
            CategoryId = 0;
        }

        public PartModel(Part part)
        {
            Id = part.Id;
            Name = part.Name;
            Price = part.Price;
            LeadTime = part.LeadTime;
            StockKeepingUnit = part.StockKeepingUnit;
            Image = part.ImagePath != null ? new Image(part.ImagePath) : new Image();
            CategoryId = part.Category.Id;
            Category = part.Category.Name;

            IsIncompatible = false;
            IsSelected = false;
        }

        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public int LeadTime { get; set; }
        [Required]
        public string StockKeepingUnit { get; set; }
        public Image Image { get; set; }
        public int CategoryId { get; set; }
        [RequiredIf("CategoryId", 0)]
        public string Category { get; set; }
        public bool IsIncompatible { get; set; }
        public bool IsSelected { get; set; }
    }
}