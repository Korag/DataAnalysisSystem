using DataAnalysisSystem.DTO.AnalysisParametersDTO;
using System.ComponentModel.DataAnnotations;

namespace DataAnalysisSystem.DTO.AnalysisDTO
{
    public class PerformNewAnalysisViewModel
    {
        public PerformNewAnalysisViewModel()
        {
            this.SelectedAnalysisMethods = new string[] { };
        }

        public string DatasetIdentificator { get; set; }

        [Required(ErrorMessage = "Field \"{0}\" is required.")]
        [Display(Name = "Name")]
        public string AnalysisName { get; set; }

        public string[] SelectedAnalysisMethods { get; set; }
        public AddAnalysisParametersViewModel AnalysisParameters { get; set; }
    }
}
