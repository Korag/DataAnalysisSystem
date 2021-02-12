using System.ComponentModel.DataAnnotations;

namespace DataAnalysisSystem.DTO.UserDTO
{
    public class EmailConfirmationViewModel
    {
        [Required(ErrorMessage = "Field \"{0}\" is required.")]
        public string UserIdentificator { get; set; }

        [Required(ErrorMessage = "Field \"{0}\" is required.")]
        public string AuthorizationToken { get; set; }
    }
}
