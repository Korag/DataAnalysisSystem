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
            this.AdditionalParameters = new DatasetAdditionalParametersViewModel()
            {
                Delimiter = ","
            };
        }

        [Required(ErrorMessage = "Field \"{0}\" is required.")]
        [Display(Name = "Name")]
        public string DatasetName { get; set; }

        [Required(ErrorMessage = "Field \"{0}\" is required.")]
        [Display(Name = "Upload dataset")]
        public IFormFile DatasetFile { get; set; }

        public DatasetAdditionalParametersViewModel AdditionalParameters { get; set; }

        
        public DatasetContent DatasetContent { get; set; }
        public string InputFileName { get; set; }
        public string InputFileFormat { get; set; }

    }
}
