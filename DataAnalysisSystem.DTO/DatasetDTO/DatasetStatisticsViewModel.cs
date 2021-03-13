using System.ComponentModel.DataAnnotations;

namespace DataAnalysisSystem.DTO.DatasetDTO
{
    public class DatasetDetailsStatisticsViewModel
    {
        [Display(Name = "Number of columns")]
        public int NumberOfColumns { get; set; }
        [Display(Name = "Number of rows")]
        public int NumberOfRows { get; set; }

        [Display(Name = "Number of missing values")]
        public int NumberOfMissingValues { get; set; }

        [Display(Name = "Input file format")]
        public string InputFileFormat { get; set; }
        [Display(Name = "Input file name")]
        public string InputFileName { get; set; }

        //Collections for Chart - attribute string to attribute value
        //Percentage chart of missing values
    }
}
