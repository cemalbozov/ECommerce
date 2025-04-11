using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Entity.DTO
{
    public record ProductDtoForUpdate : ProductDtoForManipulation
    {
        [Required]
        public int Id { get; init; }
    }
}
