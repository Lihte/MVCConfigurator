using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCConfigurator.Domain.Models
{
    public class Part : EntityBase
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int LeadTime { get; set; }
        public string StockKeepingUnit { get; set; }
        public string ImagePath { get; set; }
        public virtual PartCategory Category{ get; set; }
        public virtual IList<Part> IncompatibleParts { get; set; }
    }
}
