using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Entity.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public int? ParentCategoryId { get; set; }
        public ParentCategory ParentCategory { get; set; }
        public List<ProductCategory> ProductCategories { get; set; }

    }
}
