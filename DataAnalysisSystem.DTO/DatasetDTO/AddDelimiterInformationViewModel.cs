using System.ComponentModel.DataAnnotations;

namespace DataAnalysisSystem.DTO.DatasetDTO
{
    public class AddDelimiterInformationViewModel
    {
        public string DatasetName { get; set; }

        public byte[] DatasetContentByteArray { get; set; }
        public string DatasetContentString { get; set; }

        public string InputFileName { get; set; }
        public string InputFileFormat { get; set; }


        [Required(ErrorMessage = "Field \"{0}\" is required.")]
        [Display(Name = "Csv file delimiter")]
        public string CsvDelimiter { get; set; }
    }
}
