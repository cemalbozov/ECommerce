﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Entity.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
    }
}
