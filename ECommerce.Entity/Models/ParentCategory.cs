using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Entity.Models
{
    public class ParentCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Category> Categories { get; set; }
    }
}
