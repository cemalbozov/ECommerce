﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Entity.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public List<CartItem> CartItems { get; set; }
    }
}
