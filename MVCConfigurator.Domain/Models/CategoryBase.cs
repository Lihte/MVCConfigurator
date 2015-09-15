using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCConfigurator.Domain.Models
{
    public abstract class CategoryBase : EntityBase
    {
        public string Name { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            CategoryBase c = obj as CategoryBase;

            if ((System.Object)c == null)
                return false;

            return Name == c.Name;
        }
    }
}
