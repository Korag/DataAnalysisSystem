using System.ComponentModel.DataAnnotations;

namespace DataAnalysisSystem.DTO.DatasetDTO
{
    public class DatasetOverallInformationViewModel
    {
        public string DatasetIdentificator { get; set; }

        [Display(Name = "Dataset name")]
        public string DatasetName { get; set; }
        [Display(Name = "Creation at")]
        public string DateOfCreation { get; set; }
        [Display(Name = "Edition at")]
        public string DateOfEdition { get; set; }

        [Display(Name = "Number of columns")]
        public int NumberOfColumns { get; set; }
        [Display(Name = "Number of rows")]
        public int NumberOfRows { get; set; }

        [Display(Name = "Input file format")]
        public string InputFileFormat { get; set; }
        [Display(Name = "Input file name")]
        public string InputFileName { get; set; }
    }
}
