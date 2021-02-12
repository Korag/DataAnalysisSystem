using System.ComponentModel.DataAnnotations;

namespace DataAnalysisSystem.DTO.UserDTO
{
    public class ChangeForgottenPasswordViewModel
    {
        public ChangeForgottenPasswordViewModel(string userIdentificator, string authorizationToken)
        {
            this.UserIdentificator = userIdentificator;
            this.AuthorizationToken = authorizationToken;
        }

        public string UserIdentificator { get; set; }
        public string AuthorizationToken { get; set; }

        [Required(ErrorMessage = "Field \"{0}\" is required.")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "The password should be between 6 and 100 characters.", MinimumLength = 6)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Field \"{0}\" is required.")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "The password should be between 6 and 100 characters.", MinimumLength = 6)]
        [Display(Name = "Confirm your new password.")]
        [Compare("Password", ErrorMessage = "The passwords entered are different.")]
        public string ConfirmPassword { get; set; }
    }
}
