using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataAnalysisSystem.DTO.DatasetDTO
{
    public class SharedDatasetsBrowserViewModel
    {
        [Display(Name = "Access Key")]
        [Required(ErrorMessage = "Field \"{0}\" is required.")]
        [StringLength(8, ErrorMessage = "Field \"{0}\" does not contain a valid access code.")]
        public string NewSharedDatasetAccessKey { get; set; }

        public IList<SharedDatasetViewModel> SharedDatasets { get; set; }
    }
}
