﻿using System.ComponentModel.DataAnnotations;

namespace Blogger.Web.Models.ViewModel
{
    public class LoginViewModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        [MinLength(6, ErrorMessage = "Password has to be 6 characters")]
        public string Password { get; set; }
        public string ReturnUrl { get; set; }
    }
}
