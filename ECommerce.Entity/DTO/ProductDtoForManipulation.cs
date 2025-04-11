using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Entity.DTO
{
    public abstract record ProductDtoForManipulation
    {
        [Required(ErrorMessage = "Ürün adı zorunlu bir alandır.")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Ürün Adı 5-100 karakter aralığında olmalıdır")]
        public string Name { get; init; }

        [Required(ErrorMessage = "Ürün Url zorunlu bir alandır.")]
        public string Url { get; init; }

        [Required(ErrorMessage = "Ürün Fiyatı zorunlu bir alandır.")]
        [Range(1, 200000, ErrorMessage = "1-200000 arasında bir değer girmelisiniz.")]
        public double? Price { get; init; }
        public bool Discount { get; init; }

        [Range(1, 200000, ErrorMessage = "1-200000 arasında bir değer girmelisiniz.")]
        public double? DiscPrice { get; init; }
        [Required(ErrorMessage = "Ürün Açıklaması zorunlu bir alandır.")]
        public string Description { get; init; }
        public string ImageUrl { get; init; }
    }
}
