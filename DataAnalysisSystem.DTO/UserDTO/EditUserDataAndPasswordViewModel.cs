using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace DataAnalysisSystem.DTO.UserDTO
{
    public class EditUserDataAndPasswordViewModel
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


        [Required(ErrorMessage = "Field \"{0}\" is required.")]
        [StringLength(100, ErrorMessage = "The password should be between 6 and 100 characters.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Current Password")]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = "Field \"{0}\" is required.")]
        [StringLength(100, ErrorMessage = "The password should be between 6 and 100 characters.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Field \"{0}\" is required.")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The passwords entered are different.")]
        public string ConfirmNewPassword { get; set; }
    }
}
