using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace DataAnalysisSystem.DTO.UserDTO
{
    public class ChangeUserPasswordViewModel
    {
        public ObjectId UserIdentificator { get; set; }
        public bool UserDataSection { get; set; }

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
