﻿using System.ComponentModel.DataAnnotations;

namespace DataAnalysisSystem.DTO.UserDTO
{
    public class UserLoginViewModel
    {
        [Required(ErrorMessage = "Field \"{0}\" is required.")]
        [Display(Name = "Email address")]
        [EmailAddress(ErrorMessage = "Field \"{0}\" does not contain a valid email address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Field \"{0}\" is required.")]
        [StringLength(100, ErrorMessage = "The password should be between 6 and 100 characters.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me")]
        public bool RememberMe { get; set; }
    }
}
