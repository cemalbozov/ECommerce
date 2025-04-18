﻿using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace ECommerce.Entity.Models
{
    public class ProductCategory
    {
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
