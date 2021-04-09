using System.ComponentModel.DataAnnotations;

namespace DataAnalysisSystem.DTO.AnalysisParametersDTO.AddParameters
{
    public class AddBasicStatisticsParametersViewModel
    {
        [Required(ErrorMessage = "Field \"{0}\" is required.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "The first name should be between 3 and 100 characters")]
        [DataType(DataType.Text)]
        [Display(Name = "Last name")]
        public string LastName { get; set; }
    }
}
