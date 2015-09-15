using MVCConfigurator.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCConfigurator.UI.Models
{
    public class PartViewModel
    {
        public int ProductId { get; set; }
        public PartModel PartDetails {get; set;}
        public IEnumerable<SelectListItem> Categories { get; set; }
        public IList<PartModel> ExistingParts { get; set; }
    }

    public class PartModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int LeadTime { get; set; }
        public string StockKeepingUnit { get; set; }
        public string ImagePath { get; set; }
        public string Category { get; set; }
        public bool IsIncompatible { get; set; }
    }
}