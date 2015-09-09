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
        public PartModel PartDetails {get; set;}
        public IEnumerable<SelectListItem> Categories { get; set; }
    }

    public class PartModel
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int LeadTime { get; set; }
        public string StockKeepingUnit { get; set; }
        public string ImagePath { get; set; }
        public PartCategory Category { get; set; }
        public virtual IList<Part> IncompatibleParts { get; set; }
    }
}