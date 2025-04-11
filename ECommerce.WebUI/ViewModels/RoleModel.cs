using ECommerce.WebUI.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.WebUI.ViewModels
{
    public class RoleModel
    {
        [Display(Name = "Rol Adı")]
        [Required(ErrorMessage = "Rol adı zorunlu bir alandır.")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Rol Adı 2-30 karakter aralığında olmalıdır")]
        public string Name { get; set; }
    }

    public class RoleDetails
    {
        public IdentityRole Role { get; set; }
        public IEnumerable<User> Members { get; set; }
        public IEnumerable<User> NonMembers { get; set; }
    }
    public class RoleEditModel
    {
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public string[] IdsToAdd { get; set; }
        public string[] IdsToDelete { get; set; }
    }
}
