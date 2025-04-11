using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Entity.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public double Price { get; set; }
        public int? BrandId { get; set; }
        public Brand Brand { get; set; }
        public bool Discount { get; set; }
        public double? DiscPrice { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public bool IsApproved { get; set; }
        public bool IsHome { get; set; }
        public DateTime DateAdded { get; set; }
        public List<ProductCategory> ProductCategories { get; set; }
        public List<ProductAttribute> ProductAttributes { get; set; }
    }
}
