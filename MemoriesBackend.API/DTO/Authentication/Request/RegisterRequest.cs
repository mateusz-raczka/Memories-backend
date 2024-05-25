﻿using System.ComponentModel.DataAnnotations;

namespace MemoriesBackend.API.DTO.Authentication.Request
{
    public class RegisterRequest
    {
        [Required(ErrorMessage = "Username is required")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}