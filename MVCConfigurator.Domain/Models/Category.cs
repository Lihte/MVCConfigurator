﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCConfigurator.Domain.Models
    {
    public class Category:EntityBase
        {
        public string Name { get; set; }

        public override bool Equals(object obj)
            {
            if(obj == null)
                return false;

            Category c = obj as Category;

            if((System.Object)c == null)
                return false;

            return Id == c.Id && Name == c.Name;
            }
        }
    }