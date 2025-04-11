using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Entity.DTO
{
    public record TokenDto
    {
        public String AccessToken { get; init; }
        public String RefreshToken { get; init; }
    }
}
