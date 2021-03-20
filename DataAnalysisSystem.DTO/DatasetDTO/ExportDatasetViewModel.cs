using System.ComponentModel.DataAnnotations;

namespace DataAnalysisSystem.DTO.DatasetDTO
{
    public class ExportDatasetViewModel
    {
        public string DatasetIdentificator { get; set; }

        [Display(Name = "Dataset name")]
        public string DatasetName { get; set; }

        public DatasetContentViewModel DatasetContent { get; set; }

        public string DatasetContentFormatCSV { get; set; }
        public string DatasetContentFormatJSON { get; set; }
        public string DatasetContentFormatXLSX { get; set; }
        public string DatasetContentFormatXML { get; set; }
    }
}
