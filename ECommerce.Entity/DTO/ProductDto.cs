using System;
using System.Collections.Generic;

namespace ECommerce.Entity.DTO
{
    public record ProductDto
    {
        public int Id { get; init; }
        public string Name { get; init; }
        public string Url { get; init; }
        public double? Price { get; init; }
        public bool Discount { get; init; }
        public double? DiscPrice { get; init; }
        public string Description { get; init; }
        public string ImageUrl { get; init; }
        public ICollection<CategoryDto> Categories { get; init; }
    }
}
