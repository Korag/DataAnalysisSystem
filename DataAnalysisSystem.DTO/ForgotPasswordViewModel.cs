using System.ComponentModel.DataAnnotations;

namespace DataAnalysisSystem.DTO
{
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "Field \"{0}\" is required.")]
        [Display(Name = "Email address")]
        [EmailAddress(ErrorMessage = "Field \"{0}\" does not contain a valid email address.")]
        public string Email { get; set; }
    }
}
