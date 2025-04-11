﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Entity.Models
{
    public class ProductAttribute
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }

    }
}
