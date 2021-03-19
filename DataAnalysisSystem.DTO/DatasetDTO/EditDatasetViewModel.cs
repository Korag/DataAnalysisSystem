using System.ComponentModel.DataAnnotations;

namespace DataAnalysisSystem.DTO.DatasetDTO
{
    public class EditDatasetViewModel
    {
        public string DatasetIdentificator { get; set; }

        [Display(Name = "Dataset name")]
        public string DatasetName { get; set; }

        [Display(Name = "Edition at")]
        public string DateOfEdition { get; set; }

        public EditDatasetContentViewModel DatasetContent { get; set; }
    }
}
