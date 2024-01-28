﻿using Shopping.Enums;
using System.ComponentModel.DataAnnotations;

namespace Shopping.Models.DTO
{
    public class LoginModel
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public string ReturnUrl { get; set; }
        public LoginScenario loginScenario { get; set; }
    }
}
