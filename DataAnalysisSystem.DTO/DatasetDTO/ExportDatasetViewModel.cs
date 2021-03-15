using System.ComponentModel.DataAnnotations;

namespace DataAnalysisSystem.DTO.DatasetDTO
{
    public class ExportDatasetViewModel
    {
        public string DatasetIdentificator { get; set; }

        [Display(Name = "Dataset name")]
        public string DatasetName { get; set; }

        public DatasetContentViewModel DatasetContent { get; set; }

        //CSV
        //JSON
        //XLSX
        //XML
    }
}
