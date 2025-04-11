using ECommerce.Entity.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.WebUI.ViewModels
{
    public class ProductModel
    {
        public int Id { get; set; }
        [Display(Name = "Ürün Adı")]
        [Required(ErrorMessage ="Ürün adı zorunlu bir alandır.")]
        [StringLength(100,MinimumLength =5, ErrorMessage ="Ürün Adı 5-100 karakter aralığında olmalıdır")]
        public string Name { get; set; }
        [Display(Name = "Ürün Url")]
        [Required(ErrorMessage = "Ürün Url zorunlu bir alandır.")]
        public string Url { get; set; }
        [Display(Name = "Ürün Fiyatı")]
        [Required(ErrorMessage = "Ürün Fiyatı zorunlu bir alandır.")]
        [Range(1,200000,ErrorMessage ="1-200000 arasında bir değer girmelisiniz.")]
        public double? Price { get; set; }
        public int BrandId { get; set; }
        [Display(Name = "Ürün Markası")]
        public Brand Brand { get; set; }
        public bool Discount { get; set; }
        public double? DiscPrice { get; set; }
        [Display(Name = "Ürün Açıklaması")]
        [Required(ErrorMessage = "Ürün Açıklaması zorunlu bir alandır.")]
        public string Description { get; set; }
        [Display(Name = "Ürün Resmi")]
        public string ImageUrl { get; set; }
        [Display(Name = "Yayında mı?")]
        public bool IsApproved { get; set; }
        [Display(Name = "Anasayfada mı?")]
        public bool IsHome { get; set; }
        [Display(Name = "Kategorileri")]
        public List<Category> SelectedCategories { get; set; }
        [Display(Name = "Özellikleri")]
        public List<ProductAttribute> ProductAttributes { get; set; }
        public List<Brand> AllBrands { get; set; }
    }
}
