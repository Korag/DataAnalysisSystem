using System.ComponentModel.DataAnnotations;

namespace DataAnalysisSystem.DTO.DatasetDTO
{
    public class DatasetAdditionalParametersViewModel
    {
        [Display(Name = "Delimiter")]
        public string Delimiter { get; set; }
    }
}
