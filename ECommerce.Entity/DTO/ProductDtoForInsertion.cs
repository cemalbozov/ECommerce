using System.Collections;
using System.Collections.Generic;

namespace ECommerce.Entity.DTO
{
    public record ProductDtoForInsertion : ProductDtoForManipulation
    {
        public ICollection<int> CategoryIds { get; init; }
    }
}
