using ECommerce.Entity.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.WebUI.ViewModels
{
    public class CategoryModel
    {
        public int Id { get; set; }
        [Display(Name = "Kategori Adı")]
        [Required(ErrorMessage = "Kategori adı zorunlu bir alandır.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Kategori Adı 3-100 karakter aralığında olmalıdır")]
        public string Name { get; set; }
        [Display(Name = "Kategori Url")]
        [Required(ErrorMessage = "Kategori Url zorunlu bir alandır.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Kategori Url 3-100 karakter aralığında olmalıdır")]
        public string Url { get; set; }
        public List<Product> Products { get; set; }
        [Display(Name = "Üst Kategori")]
        public ParentCategory ParentCategory { get; set; }
        public List<ParentCategory> ParentCategories { get; set; }
    }
}
