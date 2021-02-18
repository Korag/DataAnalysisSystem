using DataAnalysisSystem.DataEntities;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataAnalysisSystem.DTO.DatasetDTO
{
    public class AddNewDatasetViewModel
    {
        public AddNewDatasetViewModel()
        {
            this.DatasetContent = new List<DatasetColumnAbstract>();
        }

        [Required(ErrorMessage = "Field \"{0}\" is required.")]
        [Display(Name = "Name")]
        public string DatasetName { get; set; }

        [Required(ErrorMessage = "Field \"{0}\" is required.")]
        [Display(Name = "Upload dataset")]
        public IFormFile DatasetFile { get; set; }

        [Display(Name = "Loaded Dataset")]
        public ICollection<DatasetColumnAbstract> DatasetContent { get; set; }
    }
}
