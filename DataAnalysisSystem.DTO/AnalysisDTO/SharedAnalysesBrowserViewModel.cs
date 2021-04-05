using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataAnalysisSystem.DTO.AnalysisDTO
{
    public class SharedAnalysesBrowserViewModel
    {
        [Display(Name = "Access Key")]
        [Required(ErrorMessage = "Field \"{0}\" is required.")]
        [StringLength(8, ErrorMessage = "Field \"{0}\" does not contain a valid access code.")]
        public string NewSharedAnalysisAccessKey { get; set; }

        public IList<SharedAnalysisByCollabViewModel> SharedAnalyses { get; set; }
    }
}
