﻿using System.ComponentModel.DataAnnotations;

namespace ECommerce.Entity.DTO
{
    public record UserForAuthenticationDto
    {
        [Required(ErrorMessage = "Email is required.")]
        public string Email { get; init; }

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; init; }
    }
}
