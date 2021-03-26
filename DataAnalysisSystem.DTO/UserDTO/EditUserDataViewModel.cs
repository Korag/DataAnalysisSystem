﻿using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace DataAnalysisSystem.DTO.UserDTO
{
    public class EditUserDataViewModel
    {
        public ObjectId UserIdentificator { get; set; }
        public bool UserDataSection { get; set; }

        [Required(ErrorMessage = "Field \"{0}\" is required.")]
        [EmailAddress(ErrorMessage = "Field \"{0}\" does not contain a valid email address.")]
        [Display(Name = "Email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Field \"{0}\" is required.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "The first name should be between 3 and 100 characters")]
        [DataType(DataType.Text)]
        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Field \"{0}\" is required.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "The last name should be between 3 and 100 characters.")]
        [DataType(DataType.Text)]
        [Display(Name = "Last name")]
        public string LastName { get; set; }
    }
}
